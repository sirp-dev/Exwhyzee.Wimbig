using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exwhyzee.Wimbig.Notification.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Exwhyzee.Wimbig.Data.Repository.MessageStores;
using Exwhyzee.Wimbig.Application;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;
using Hangfire.Dashboard;

namespace Exwhyzee.Wimbig.Notification.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //"DefaultConnection": "Server=SQL5033.site4now.net;Database=DB_A440E4_WimbigServiceDb;user=DB_A440E4_WimbigServiceDb_admin;Password=wbsl@xyz247;MultipleActiveResultSets=true"

            services.AddHangfire(x => x.UseSqlServerStorage(nameOrConnectionString: "Data Source=SQL5033.site4now.net;Initial Catalog=DB_A440E4_WimbigServiceDb;User Id=DB_A440E4_WimbigServiceDb_admin;Password=wbsl@xyz247;MultipleActiveResultSets=true"));

            services.AddHangfireServer();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
          
            services.AddWimbigApplicationServices();
            services.AddScoped<IMessageStoreRepository, MessageStoreRepository>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = 443;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMessageService _message, ILogger<Startup> logger)
        {

            app.UseHangfireServer();
            //app.UseHangfireDashboard("/xyz");
            app.UseHangfireDashboard("/hangfire", options: new DashboardOptions()
            {
                Authorization = new IDashboardAuthorizationFilter[]
      {
                new MyAuthorizationFilter()
      }
            });

            //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            RecurringJob.AddOrUpdate(recurringJobId: "message-send", methodCall: () => _message.SendMessageMethod(), Cron.Minutely);
            //BackgroundJob.Schedule(() => _message.SendMessageMethod(), TimeSpan.FromTicks(60));
            loggerFactory.AddSerilog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Message/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

           
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            //app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                       name: "areas",
                        template: "{area:exists}/{controller=Message}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Message}/{action=Index}/{id?}");
            });
        }

        
    }

    class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            //var httpContext = context.GetHttpContext();

            //// Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}
