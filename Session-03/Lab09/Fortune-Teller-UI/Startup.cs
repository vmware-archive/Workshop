using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Fortune_Teller_Service.Common.Services;
using Pivotal.Extensions.Configuration;
using Pivotal.Discovery.Client;
using Steeltoe.CloudFoundry.Connector.Redis;
using Steeltoe.Security.DataProtection;
using Microsoft.AspNetCore.DataProtection;

namespace Fortune_Teller_UI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                // Lab07 Start
                .AddConfigServer(env)
                // Lab07 End
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Lab09 Start
            if (Environment.IsProduction())
            {
                // Use Redis cache on CloudFoundry to DataProtection Keys
                services.AddRedisConnectionMultiplexer(Configuration);
                services.AddDataProtection()
                    .PersistKeysToRedis()
                    .SetApplicationName("fortuneui");

                // Use Redis cache on CloudFoundry to store session data
                services.AddDistributedRedisCache(Configuration);
            }
            // Lab09 End
       
            // Lab06 Start
            services.AddSingleton<IFortuneService, FortuneServiceClient>();
            // Lab06 End

            // Lab07 Start
            services.Configure<FortuneServiceConfig>(Configuration.GetSection("fortuneService"));
            // Lab07 End

            // Lab08 Start
            services.AddDiscoveryClient(Configuration);
            // Lab08 End

            // Add framework services.
            services.AddSession();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Fortunes}/{action=Index}/{id?}");
            });

            // Lab08 Start
            app.UseDiscoveryClient();
            // Lab08 End
        }
    }
}
