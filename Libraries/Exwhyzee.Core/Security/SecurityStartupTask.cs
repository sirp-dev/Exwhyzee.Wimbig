using Exwhyzee.Configuration;
using Exwhyzee.Tasks;
using Exwhyzee.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Exwhyzee.Security
{
    public class SecurityStartupTask : IStartupTask
    {
        public int Order => 0;

        public async Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var settingsService = serviceProvider.GetService<ISettingsService>();
            var securitySettings = await settingsService.Load<SecuritySettings>();
            
            if (string.IsNullOrWhiteSpace(securitySettings.EncryptionKey))
            {
                //Generate encryption key
                securitySettings.EncryptionKey = CommonHelper.GenerateRandomDigitCode(16);
                await settingsService.Update(securitySettings);
            }

        }
    }
}
