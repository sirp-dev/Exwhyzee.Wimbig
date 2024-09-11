using Exwhyzee.Configuration;

namespace Exwhyzee.Security
{
    public class SecuritySettings : ISettings
    {        
        /// <summary>
        /// Gets or sets an encryption key
        /// </summary>
        public string EncryptionKey { get; set; }
    }
}
