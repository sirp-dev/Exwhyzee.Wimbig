using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Exwhyzee.Wimbig.Web.Areas.Identity.IdentityHostingStartup))]
namespace Exwhyzee.Wimbig.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}