using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Exwhyzee.Exceptions
{
    public static class ExceptionExtensions
    {
        public static bool IsFatal(this Exception e)
        {
            return
                e is InsufficientExecutionStackException ||
                //ex is StackOverflowException ||
                e is OutOfMemoryException ||
                e is UnauthorizedAccessException ||
                //ex is AppDomainUnloadedException ||
                //ex is   ThreadAbortException ||
                e is SecurityException ||
                e is SEHException;
        }


        /// <summary>
        /// Gets a message that describes the current exception and it's inner exception.
        /// </summary>
        /// <param name="e">The <see cref="Exception"/></param>
        /// <returns>The error message that explains the reason for the exception and it's inner exception, or an empty string</returns>
        public static string GetFullMessage(this Exception e)
        {
            if (e == null)
                return null;

            string fullMessage = e.Message;
            if (e.InnerException != null)
            {
                fullMessage += $" {e.InnerException.Message}";
            }

            return fullMessage;
        }
    }
}
