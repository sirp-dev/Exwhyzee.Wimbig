using Exwhyzee.Domain;
using Exwhyzee.Utilities;
using Dapper;
using Exwhyzee.Caching;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Data;

namespace Exwhyzee.Configuration
{
    public class SettingsService : ISettingsService
    {
        #region Const

        private const string CACHE_SETTINGS = "anq.settings";
        private const int CACHE_EXPIRATION_MINUTES = 30;
        private const string TABLE_NAME = "Settings";

        #endregion

        #region Fields

        private readonly IStorage _storage;
        private readonly IJsonConverter _jsonConverter;
        private readonly IMemoryCache _memoryCache;
        private readonly ISignal _signal;
        private readonly IClock _clock;

        #endregion

        #region Ctor

        public SettingsService(
            IStorage storage,
            IJsonConverter jsonConverter,
            IMemoryCache memoryCache,
            ISignal signal,
            IClock clock)
        {
            _storage = storage;
            _jsonConverter = jsonConverter;
            _memoryCache = memoryCache;
            _signal = signal;
            _clock = clock;
        }

        #endregion

        #region Utils

        private JObject LoadJObject<TSettings>(TSettings settings)
        {
            var json = _jsonConverter.Serialize(settings);
            return JObject.Parse(json);
        }

        #endregion

        #region Methods

        public async Task<TSettings> Load<TSettings>() where TSettings : ISettings, new()
        {
            string cacheKey = $"{CACHE_SETTINGS}.settings:{typeof(TSettings).FullName}";

            return await _memoryCache.GetOrCreateAsync(cacheKey,async (entry) =>
            {
                var jObject = LoadJObject(new TSettings());

                var settingNames = jObject.Properties().Select(p => p.Name);
                var dbSettingNamePrefix = typeof(TSettings).Name;
                var dbSettingNames = settingNames.Select(name => ($"{dbSettingNamePrefix}.{name}").ToLowerInvariant());

                string joinedDbSettingNames = string.Join(",", dbSettingNames.Select(name => $"'{name}'"));

                var dbSettings = await _storage.UseConnection(conn =>
                {
                    string sql = $"select * from {TABLE_NAME} where [name] in ({joinedDbSettingNames})";
                    var query = conn.Query<Setting>(sql,
                        new { },
                        commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS
                        );

                    return Task.FromResult(query.ToList());
                });

                if (dbSettings == null || dbSettings.Count == 0)
                    return await Task.FromResult(jObject.ToObject<TSettings>());

                foreach (var settingName in settingNames)
                {
                    var dbSetting = dbSettings.FirstOrDefault(d =>
                        d.Name.Equals($"{dbSettingNamePrefix}.{settingName}", StringComparison.OrdinalIgnoreCase));

                    if (dbSetting == null)
                        continue;

                    jObject[settingName] = dbSetting.Value;
                }

                entry.AbsoluteExpiration = _clock.UtcNow.AddMinutes(CACHE_EXPIRATION_MINUTES);
                entry.ExpirationTokens.Add(_signal.GetToken(CACHE_SETTINGS));
                return await Task.FromResult(jObject.ToObject<TSettings>());
            });
        }

        public async Task Update<TSettings>(TSettings settings) where TSettings : ISettings, new()
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            JObject jObject = LoadJObject(settings);

            var settingNames = jObject.Properties().Select(p => p.Name);
            var dbSettingNamePrefix = typeof(TSettings).Name;
            var dbSettingNames = settingNames.Select(name => ($"{dbSettingNamePrefix}.{name}").ToLowerInvariant());

            string joinedDbSettingNames = string.Join(",", dbSettingNames.Select(name => $"'{name}'"));
            var dbSettingRecords = jObject.Properties()
                .Select(x => $"('{dbSettingNamePrefix.ToLowerInvariant()}.{x.Name.ToLowerInvariant()}','{x.Value}')");
            string joinedDbSettingRecords = string.Join(",", dbSettingRecords);

            _storage.UseConnection(conn =>
            {
                var transaction = conn.BeginTransaction();

                string sql = $"delete from {TABLE_NAME} where name in ({joinedDbSettingNames})";
                conn.Execute(sql,
                    commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS,
                    transaction: transaction
                    );

                sql = $"insert into {TABLE_NAME}(name,value) values {joinedDbSettingRecords}";
                conn.Execute(sql,
                    commandTimeout: DataConstants.COMMAND_TIMEOUT_SECONDS,
                    transaction: transaction
                    );

                transaction.Commit();
            });

            _signal.SignalToken(CACHE_SETTINGS);
            await Task.CompletedTask;
        }

        #endregion
    }
}
