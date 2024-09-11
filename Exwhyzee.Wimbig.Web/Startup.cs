using Exwhyzee.Wimbig.Application;
using Exwhyzee.Wimbig.Application.MessageStores;
using Exwhyzee.Wimbig.Core.Authorization.Users;
using Exwhyzee.Wimbig.Data;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Exwhyzee.Wimbig.Hangfire.Core.SMS;
using Exwhyzee.Wimbig.NotificationService.Abstract;
using Exwhyzee.Wimbig.Web.Areas.Identity.Services;
using Exwhyzee.Wimbig.Web.Hubs;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace Exwhyzee.Wimbig.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHangfire(x => x.UseSqlServerStorage(nameOrConnectionString: "Data Source=SQL5033.site4now.net;Initial Catalog=DB_A440E4_WimbigServiceDb;User Id=DB_A440E4_WimbigServiceDb_admin;Password=wbsl@xyz247;MultipleActiveResultSets=true"));
            services.AddHangfireServer();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var now = services.AddDbContext<WimbigDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            var dbconnection = now;

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<WimbigDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
               // options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddRazorPagesOptions(options =>
             {
                 options.AllowAreas = true;
                 options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                 options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
             });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            // using Microsoft.AspNetCore.Identity.UI.Services;
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddWimbigApplicationServices();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IMessageStoreRepository, MessageStoreRepository>();
            //services.AddScoped<NotificationSender, INotificationSender>();
            services.AddHttpClient<MessageStoreAppService>();


            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = 443;
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env, ILoggerFactory loggerFactory, IMessageStoreRepository _message)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            //RecurringJob.AddOrUpdate(recurringJobId: "message-send", methodCall: () => _message.SendMessageMethod(), Cron.Minutely);
            //BackgroundJob.Schedule(() => _message.SendMessageMethod(), TimeSpan.FromTicks(60));


            loggerFactory.AddSerilog();
            if (env.IsDevelopment())
            {
                //var options = new RewriteOptions();
                //options.AddRedirectToHttps();
                //options.Rules.Add(new RedirectToWwwRule());
                //app.UseRewriter(options);

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
               
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<RaffleHub>("/raffleHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                       name: "areas",
                        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
