using Exwhyzee.Wimbig.Application;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeterKottas.DotNetCore.WindowsService;
using System;

namespace Exwhyzee.Wimbig.GameStatsTransactions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var services = new ServiceCollection();
            ConfigureServices(services);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var ticketAppService = serviceProvider.GetService<IPurchaseTicketAppService>();
            
            ServiceRunner<GameStatsTransactionService>.Run(config =>
            {
                var name = config.GetDefaultName();
                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((extraArguments, controller) =>
                    {
                        return new GameStatsTransactionService(controller,ticketAppService);
                    });

                    serviceConfig.OnStart((service, extraParams) =>
                    {
                        Console.WriteLine("Service {0} started", name);
                        service.Start();
                    });

                    serviceConfig.OnStop(service =>
                    {
                        Console.WriteLine("Service {0} stopped", name);
                        service.Stop();
                    });

                    serviceConfig.OnShutdown(service =>
                    {
                        Console.WriteLine("Service {0} shutdown", name);

                    });

                    serviceConfig.OnError(e =>
                    {

                        Console.WriteLine("Service {0} errored with exception : {1}", name, e.Message);
                    });
                });
            });
        }

        public static IConfiguration Configuration { get; set; }
        public static void ConfigureServices(IServiceCollection services)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                   .SetBasePath(AppContext.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddEnvironmentVariables();

                Configuration = configuration.Build();
                // Resolve Injection for Configuration file
                services.AddSingleton(Configuration);

                services.AddMemoryCache();

                services.AddOptions();
                services.AddLogging();
                services.AddWimbigApplicationServices();
                


            }
            catch (Exception ex)
            {

            }
        }
    }
}
