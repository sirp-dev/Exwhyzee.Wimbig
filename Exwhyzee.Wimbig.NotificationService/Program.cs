using Exwhyzee.Wimbig.Core.MessageStores;
using Exwhyzee.Wimbig.Data;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Exwhyzee.Wimbig.NotificationService.Abstract;
using Exwhyzee.Wimbig.NotificationService.Service;
using Exwhyzee.Wimbig.NotificationService.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PeterKottas.DotNetCore.WindowsService;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Exwhyzee.Wimbig.NotificationService
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var messageStoreRepository = serviceProvider.GetService<IMessageStoreRepository>();
            var senderFactory = serviceProvider.GetService<Func<MessageChannel, INotificationSender>>();

            ServiceRunner<NotificationSerice>.Run(config =>
            {
                var name = config.GetDefaultName();
                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((extraArguments, controller) =>
                    {
                        return new NotificationSerice(controller, messageStoreRepository, senderFactory);
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
                        Console.WriteLine("Service {0} errored with exception : {1}\nStack Trace {2}", name, e.Message, e);
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
                services.AddSingleton(Configuration);
                services.AddMemoryCache();
                services.AddOptions();
                services.AddLogging();
                
                services.Configure<MailSetting>(Configuration.GetSection("Smtp"));
                services.Configure<SMSSetting>(Configuration.GetSection("SMS"));

                services.AddSingleton<EmailService>();
                services.AddSingleton<SMSService>();
                HttpClient httpClient = new HttpClient();
                services.AddSingleton(httpClient);

                services.AddTransient<Func<MessageChannel, INotificationSender>>(serviceProvider => key =>
                {
                    Console.WriteLine($"Notification Sender Tranient: {key}");
                    switch (key)
                    {
                        case MessageChannel.Email:
                            return serviceProvider.GetService<EmailService>();
                        case MessageChannel.SMS:
                            return serviceProvider.GetService<SMSService>();
                        default:
                            throw new KeyNotFoundException("Transient Error"); // or maybe return null, up to you
                    }
                });

                services.AddWimbigDataServices();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
