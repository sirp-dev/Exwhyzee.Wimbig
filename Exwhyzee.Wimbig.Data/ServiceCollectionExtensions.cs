using Exwhyzee.Extensions.DependencyInjection;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Exwhyzee.Wimbig.Data.Repository.Categorys;
using Exwhyzee.Wimbig.Data.Repository.ImageFiles;
using Exwhyzee.Wimbig.Data.Repository.ImageFiles.RaffleImage;
using Exwhyzee.Wimbig.Data.Repository.MapRaffleToCategorys;
using Exwhyzee.Wimbig.Data.Repository.Raffles;
using Exwhyzee.Wimbig.Data.Repository.Sections;
using Exwhyzee.Wimbig.Data.Repository.Tickets;
using Exwhyzee.Wimbig.Data.Repository.Transactions;
using Exwhyzee.Wimbig.Data.Repository.Wallets;
using Exwhyzee.Wimbig.Data.Repository.Wimbank;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Exwhyzee.Wimbig.Data.Repository.SideBarImages;
using Exwhyzee.Wimbig.Data.Repository.BarnerImages;
using Exwhyzee.Wimbig.Data.Repository.Count;
using Exwhyzee.Wimbig.Data.Repository.DailyStatistics;
using Exwhyzee.Wimbig.Data.Repository.Agent;
using Exwhyzee.Wimbig.Data.Repository.WinnerReports;
using Exwhyzee.Wimbig.Data.Repository.PayOutReports;
using Exwhyzee.Wimbig.Data.Repository.YoutubeLink;
using Exwhyzee.Wimbig.Data.Repository.SmsMessages;
using Exwhyzee.Wimbig.Data.Repository.Cities;

namespace Exwhyzee.Wimbig.Data
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add WImbig Data services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddWimbigDataServices(this IServiceCollection services)
        {
            services.AddExwhyzeeData();
            services.AddExwhyzeeCoreServices();
            services.AddTransient<IRaffleRepository, RaffleRepository>();
            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ISectionRepository, SectionRepository>();
            services.AddTransient<ICategoryRepository,CategoryRepository>();
            services.AddTransient<IMapRaffleToCategoryRepository, MapRaffleToCategoryRepository>();
            services.AddTransient<IMessageStoreRepository, MessageStoreRepository>();
            services.AddTransient<IImageFileRepository, ImageFileRepository>();
            services.AddTransient<IMapImageToRaffleRepo, MapImageToRaffleRepo>();
            services.AddTransient<ITicketRepository, TicketRepository>();
            services.AddTransient<IWimbankRepository, WimbankRepository>();
            services.AddTransient<ISideBarImageRepository, SideBarImageRepository>();
            services.AddTransient<IBarnerRepository, BarnerRepository>();
            services.AddTransient<ICountRepository, CountRepository>();
            services.AddTransient<IDailyStatisticsRepository, DailyStatisticsRepository>();
            services.AddTransient<IAgentRepository, AgentRepository>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<IWinnerReportsRepository, WinnerReportsRepository>();
            services.AddTransient<IPayOutReportRepository, PayOutReportRepository>();
            services.AddTransient<IYoutubeLinkRepository, YoutubeLinkRepository>();
            services.AddTransient<ISmsMessageRepository, SmsMessageRepository>();
            return services;

        }
    }
}
