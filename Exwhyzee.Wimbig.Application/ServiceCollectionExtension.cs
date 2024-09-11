using Exwhyzee.Wimbig.Application.Agent;
using Exwhyzee.Wimbig.Application.Barner;
using Exwhyzee.Wimbig.Application.Categories;
using Exwhyzee.Wimbig.Application.Cities;
using Exwhyzee.Wimbig.Application.Count;
using Exwhyzee.Wimbig.Application.DailyStatistics;
using Exwhyzee.Wimbig.Application.Images;
using Exwhyzee.Wimbig.Application.MapRaffleToCategorys;
using Exwhyzee.Wimbig.Application.MessageStores;
using Exwhyzee.Wimbig.Application.PayOutReports;
using Exwhyzee.Wimbig.Application.Paystack;
using Exwhyzee.Wimbig.Application.RaffleImages;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Sections;
using Exwhyzee.Wimbig.Application.SmsMessages;
using Exwhyzee.Wimbig.Application.Tickets;
using Exwhyzee.Wimbig.Application.Transactions;
using Exwhyzee.Wimbig.Application.Wimbank;
using Exwhyzee.Wimbig.Application.WinnerReports;
using Exwhyzee.Wimbig.Application.YoutubeLink;
using Exwhyzee.Wimbig.Data;
using Exwhyzee.Wimbig.Data.Repository.PayOutReports;
using Microsoft.Extensions.DependencyInjection;

namespace Exwhyzee.Wimbig.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddWimbigApplicationServices(this IServiceCollection services)
        {
            services.AddWimbigDataServices();
            services.AddTransient<IRaffleAppService, RaffleAppService>();
            services.AddTransient<IWalletAppService, WalletAppService>();
            services.AddTransient<ITransactionAppService, TransactionAppService>();
            services.AddTransient<IPaystackTransactionAppService, PaystackTransactionAppService>();
            services.AddTransient<ISectionAppService, SectionAppService>();
            services.AddTransient<ICategoryAppService, CategoryAppService>();
            services.AddTransient<IMapRaffleToCategoryAppService, MapRaffleToCategoryAppService>();
            services.AddTransient<IMapImageToRaffleAppService, MapImageToRaffleAppService>();
            services.AddTransient<IImageFileAppService, ImageFileAppService>();
            services.AddTransient<IPurchaseTicketAppService, PurchaseTicketAppService>();
            services.AddTransient<IPurchaseTicketAppService, PurchaseTicketAppService>();
            services.AddTransient<IWimbankAppService, WimbankAppService>();
            services.AddTransient<IBarnerAppService, BarnerAppService>();
            services.AddTransient<ICountAppService, CountAppService>();
            services.AddTransient<IDailyStatisticsAppService, DailyStatisticsAppService>();
            services.AddTransient<IAgentAppService, AgentAppService>();
            services.AddTransient<IWinnerReportsAppService, WinnerReportsAppService>();
            services.AddTransient<IPayOutReportsAppService, PayOutReportsAppService>();
            services.AddTransient<IYoutubeLinkAppService, YoutubeLinkAppService>();
            services.AddTransient<ISmsMessageAppService, SmsMessageAppService>();
            services.AddTransient<IMessageStoreAppService, MessageStoreAppService>();
            services.AddTransient<ICityAppService, CityAppService>();

            return services;
        }
    }
}
