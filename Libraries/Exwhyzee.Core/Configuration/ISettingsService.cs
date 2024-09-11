using System.Threading.Tasks;

namespace Exwhyzee.Configuration
{
    public interface ISettingsService
    {
        Task<TSettings> Load<TSettings>() where TSettings : ISettings, new();

        Task Update<TSettings>(TSettings setting) where TSettings : ISettings, new();
    }
}
