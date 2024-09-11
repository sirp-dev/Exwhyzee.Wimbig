﻿using Dapper;
using EXWHYZEE.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Exwhyzee.Data
{
    public class SqlServerDistributedLock : IDisposable
    {
        private static readonly TimeSpan MaxAttemptDelay = TimeSpan.FromSeconds(5);

        private const string LockMode = "Exclusive";
        private const string LockOwner = "Session";
        private const int CommandTimeoutAdditionSeconds = 1;

        private static readonly TimeSpan KeepAliveInterval = TimeSpan.FromMinutes(1);

        private static readonly IDictionary<int, string> LockErrorMessages
            = new Dictionary<int, string>
            {
                { -1, "The lock request timed out" },
                { -2, "The lock request was canceled" },
                { -3, "The lock request was chosen as a deadlock victim" },
                { -999, "Indicates a parameter validation or other call error" }
            };

        private static readonly ThreadLocal<Dictionary<string, int>> AcquiredLocks
            = new ThreadLocal<Dictionary<string, int>>(() => new Dictionary<string, int>());

        private IDbConnection _connection;
        private readonly IStorage _storage;
        private readonly string _resource;
        private readonly Timer _timer;
        private readonly object _lockObject = new object();

        private bool _completed;

        public SqlServerDistributedLock(IStorage storage, string resource, TimeSpan timeout)
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));
            if (String.IsNullOrEmpty(resource)) throw new ArgumentNullException(nameof(resource));
            if (timeout.TotalSeconds + CommandTimeoutAdditionSeconds > Int32.MaxValue) throw new ArgumentException(
                $"The timeout specified is too large. Please supply a timeout equal to or less than {Int32.MaxValue - CommandTimeoutAdditionSeconds} seconds", nameof(timeout));
            if (timeout.TotalMilliseconds > Int32.MaxValue) throw new ArgumentException(
                $"The timeout specified is too large. Please supply a timeout equal to or less than {(int)TimeSpan.FromMilliseconds(Int32.MaxValue).TotalSeconds} seconds", nameof(timeout));

            _storage = storage;
            _resource = resource;

            if (!AcquiredLocks.Value.ContainsKey(_resource) || AcquiredLocks.Value[_resource] == 0)
            {
                _connection = storage.CreateAndOpenConnection();

                try
                {
                    Acquire(_connection, _resource, timeout);
                }
                catch (Exception)
                {
                    storage.ReleaseConnection(_connection);
                    throw;
                }

                _timer = new Timer(ExecuteKeepAliveQuery, null, KeepAliveInterval, KeepAliveInterval);

                AcquiredLocks.Value[_resource] = 1;
            }
            else
            {
                AcquiredLocks.Value[_resource]++;
            }
        }

        public void Dispose()
        {
            if (_completed) return;

            _completed = true;

            if (!AcquiredLocks.Value.ContainsKey(_resource)) return;

            AcquiredLocks.Value[_resource]--;

            if (AcquiredLocks.Value[_resource] != 0) return;

            lock (_lockObject)
            {
                // Timer callback may be invoked after the Dispose method call,
                // so we are using lock to avoid unsynchronized calls.

                try
                {
                    AcquiredLocks.Value.Remove(_resource);

                    _timer?.Dispose();

                    Release(_connection, _resource);
                }
                finally
                {
                    _storage.ReleaseConnection(_connection);
                    _connection = null;
                }
            }
        }

        private void ExecuteKeepAliveQuery(object obj)
        {
            lock (_lockObject)
            {
                try
                {
                    _connection?.Execute("SELECT 1;");
                }
                catch
                {
                    // Connection is broken. This means that distributed lock
                    // was released, and we can't guarantee the safety property
                    // for the code that is wrapped with this block. So it was
                    // a bad idea to have a separate connection for just
                    // distributed lock.
                    // TODO: Think about distributed locks and connections.
                }
            }
        }

        internal static void Acquire(IDbConnection connection, string resource, TimeSpan timeout)
        {
            var started = Stopwatch.StartNew();
            var attempt = 1;

            while (started.Elapsed < timeout)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Resource", resource);
                parameters.Add("@DbPrincipal", "public");
                parameters.Add("@LockMode", LockMode);
                parameters.Add("@LockOwner", LockOwner);
                parameters.Add("@LockTimeout", 0);
                parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                connection.Execute(
                    @"sp_getapplock",
                    parameters,
                    commandTimeout: (int)timeout.TotalSeconds,
                    commandType: CommandType.StoredProcedure);

                var lockResult = parameters.Get<int>("@Result");

                if (lockResult >= 0)
                {
                    // The lock has been successfully obtained on the specified resource.
                    return;
                }

                if (lockResult == -999 /* Indicates a parameter validation or other call error. */)
                {
                    throw new SqlServerDistributedLockException(
                        $"Could not place a lock on the resource '{resource}': {(LockErrorMessages.ContainsKey(lockResult) ? LockErrorMessages[lockResult] : $"Server returned the '{lockResult}' error.")}.");
                }

                Task.Delay(ExponentialBackoff(attempt++)).Wait();
                //Threading.Sleep(ExponentialBackoff(attempt++));
            }

            throw new DistributedLockTimeoutException(resource);
        }

        internal static void Release(IDbConnection connection, string resource)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Resource", resource);
            parameters.Add("@LockOwner", LockOwner);
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            connection.Execute(
                @"sp_releaseapplock",
                parameters,
                commandType: CommandType.StoredProcedure);

            var releaseResult = parameters.Get<int>("@Result");

            if (releaseResult < 0)
            {
                throw new SqlServerDistributedLockException(
                    $"Could not release a lock on the resource '{resource}': Server returned the '{releaseResult}' error.");
            }
        }

        private static TimeSpan ExponentialBackoff(int attemptNumber)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var nextTry = rand.Next(
                (int)Math.Pow(attemptNumber, 2), (int)Math.Pow(attemptNumber + 1, 2) + 1);
            return TimeSpan.FromMilliseconds(Math.Min(nextTry, MaxAttemptDelay.TotalMilliseconds));
        }
    }

    public class DistributedLockTimeoutException : TimeoutException
    {
        public DistributedLockTimeoutException(string resource)
            : base(
                $"Timeout expired. The timeout elapsed prior to obtaining a distributed lock on the '{resource}' resource."
                )
        {
        }
    }

    public class SqlServerDistributedLockException : Exception
    {
        public SqlServerDistributedLockException(string message)
            : base(message)
        {
        }
    }
}
