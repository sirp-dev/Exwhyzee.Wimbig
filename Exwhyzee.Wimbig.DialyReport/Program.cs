using Exwhyzee.Extensions.DependencyInjection;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.DailyStatistics;
using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Data.Repository.Count;
using Exwhyzee.Wimbig.Data.Repository.DailyStatistics;
using Exwhyzee.Wimbig.Data.Repository.PayOutReports;
using Exwhyzee.Wimbig.Data.Repository.Raffles;
using Exwhyzee.Wimbig.Data.Repository.Tickets;
using Exwhyzee.Wimbig.Data.Repository.Wallets;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeterKottas.DotNetCore.WindowsService;
using System;

namespace Exwhyzee.Wimbig.DialyReport
{
  
        public class Program
        {
            public static void Main(string[] args)
            {
                var services = new ServiceCollection();
                ConfigureServices(services);

                IServiceProvider serviceProvider = services.BuildServiceProvider();

                var raffleAppService = serviceProvider.GetService<IRaffleAppService>();
                var purchaseTicketAppService = serviceProvider.GetService<IPurchaseTicketAppService>();
                var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
                var userManger = serviceProvider.GetService<UserManager<ApplicationUser>>();
                var countAppService = serviceProvider.GetService<ICountAppService>();
                var payOutReportsAppService = serviceProvider.GetService<IPayOutReportsAppService>();
                var dailyStatisticsAppService = serviceProvider.GetService<IDailyStatisticsAppService>();
                var dailyStatisticsRepository = serviceProvider.GetService<IDailyStatisticsRepository>();
                var walletAppService = serviceProvider.GetService<IWalletAppService>();
           
            ServiceRunner<DialyReport>.Run(config =>
                {
                    var name = config.GetDefaultName();
                    config.Service(serviceConfig =>
                    {
                       
                        serviceConfig.ServiceFactory((extraArguments, controller) =>
                        {
                            return new DialyReport(controller, roleManager, purchaseTicketAppService, countAppService, payOutReportsAppService, userManger, dailyStatisticsAppService, dailyStatisticsRepository, walletAppService, raffleAppService);
                        });


                        serviceConfig.OnStart((service, extraParams) =>
                        {
                            //Console.WriteLine("Service {0} started", name);
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

                    services.AddExwhyzeeCoreServices();
                    services.AddExwhyzeeData();

                    services.AddOptions();
                    services.AddLogging();

                    services.AddTransient<IRaffleAppService, RaffleAppService>();
                    services.AddTransient<IRaffleRepository, RaffleRepository>();

                    services.AddTransient<IPurchaseTicketAppService, PurchaseTicketAppService>();
                    services.AddTransient<ITicketRepository, TicketRepository>();

                    services.AddTransient<IPayOutReportsAppService, PayOutReportsAppService>();
                    services.AddTransient<IPayOutReportRepository, PayOutReportRepository>();
                    services.AddTransient<RoleManager<IdentityRole>>();
                    services.AddTransient<ICountAppService, CountAppService>();
                    services.AddTransient<ICountRepository, CountRepository>();
                services.AddTransient<IWalletAppService, WalletAppService>();
                services.AddTransient<IWalletRepository, WalletRepository>();
                services.AddTransient<IDailyStatisticsRepository, DailyStatisticsRepository>();
                    services.AddTransient<IDailyStatisticsAppService, DailyStatisticsAppService>();
                    
                    services.AddTransient<IdentityUser>();



                }
                catch (Exception ex)
                {

                }
            }
        }
}
