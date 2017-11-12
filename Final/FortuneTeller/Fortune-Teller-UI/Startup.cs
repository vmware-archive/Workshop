
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

using Pivotal.Discovery.Client;
using Steeltoe.CloudFoundry.Connector.Redis;
using Steeltoe.Security.DataProtection;
using Steeltoe.Security.Authentication.CloudFoundry;

using Fortune_Teller_UI.Services;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Management.CloudFoundry;

namespace Fortune_Teller_UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Lab09 Start
            if (!Environment.IsDevelopment())
            {
                // Use Redis cache on CloudFoundry to DataProtection Keys
                services.AddRedisConnectionMultiplexer(Configuration);
                services.AddDataProtection()
                    .PersistKeysToRedis()
                    .SetApplicationName("fortuneui");
            }
            // Lab09 End

            // Lab06 Start
            services.AddScoped<IFortuneService, FortuneServiceClient>();
            // Lab06 End

            // Lab07 Start
            services.Configure<FortuneServiceConfig>(Configuration.GetSection("fortuneService"));
            // Lab07 End

            // Lab08 Start
            services.AddDiscoveryClient(Configuration);
            // Lab08 End

            // Lab09 Start
            if (Environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                // Use Redis cache on CloudFoundry to store session data
                services.AddDistributedRedisCache(Configuration);
            }
            // Lab09 End

            // Lab10 Start
            services.AddAuthentication((options) =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CloudFoundryDefaults.AuthenticationScheme;

            })
            .AddCookie((options) =>
            {
                options.AccessDeniedPath = new PathString("/Fortunes/AccessDenied");

            })
            .AddCloudFoundryOAuth(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("testgroup", policy => policy.RequireClaim("scope", "testgroup"));
                options.AddPolicy("testgroup1", policy => policy.RequireClaim("scope", "testgroup1"));

            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Lab10 End

            // Lab11 Start
            services.AddHystrixCommand<FortuneServiceCommand>("FortuneService", Configuration);
            services.AddHystrixMetricsStream(Configuration);
            // Lab11 End

            // Add framework services.
            services.AddSession();

            // Lab12 Start
            services.AddCloudFoundryActuators(Configuration);
            // Lab12 End

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Fortunes/Error");
            }

            app.UseStaticFiles();

            // Lab12
            app.UseCloudFoundryActuators();
            // Lab12

            // Lab11 Start
            app.UseHystrixRequestContext();
            // Lab11 End

            app.UseSession();

            // Lab10 Start
            app.UseAuthentication();
            // Lab10 End

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Fortunes}/{action=Index}/{id?}");
            });


            // Lab08 Start
            app.UseDiscoveryClient();
            // Lab08 End

            // Lab11 Start
            if (!Environment.IsDevelopment())
            {
                app.UseHystrixMetricsStream();
            }
            // Lab11 End
        }
    }
}
