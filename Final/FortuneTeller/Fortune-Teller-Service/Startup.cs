using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Fortune_Teller_Service.Models;

using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Pivotal.Discovery.Client;
using Steeltoe.Security.Authentication.CloudFoundry;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.CloudFoundry;

namespace Fortune_Teller_Service
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

            // Lab09 add
            if (Environment.IsDevelopment())
            {
                // Lab06 Start
                services.AddEntityFrameworkInMemoryDatabase().AddDbContext<FortuneContext>(
                    options => options.UseInMemoryDatabase("Fortunes"), ServiceLifetime.Singleton);
                // Lab06 End
            }
            else
            {
                // Lab09 add
                services.AddDbContext<FortuneContext>(options => options.UseMySql(Configuration));
            }
            // Lab06 Start
            services.AddSingleton<IFortuneRepository, FortuneRepository>();
            // Lab06 End

            // Lab08 Start
            services.AddDiscoveryClient(Configuration);
            // Lab08 End

            // Lab10 Start
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCloudFoundryJwtBearer(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("testgroup", policy => policy.RequireClaim("scope", "testgroup"));
                options.AddPolicy("testgroup1", policy => policy.RequireClaim("scope", "testgroup1"));
            });
            // Lab10 End

            // Lab12 Start
            services.AddSingleton<IHealthContributor, MySqlHealthContributor>();
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
            }

            // Lab12
            app.UseCloudFoundryActuators();
            // Lab12

            // Lab10 Start
            app.UseAuthentication();
            // Lab10 End

            app.UseMvc();

            // Lab08 Start
            app.UseDiscoveryClient();
            // Lab08 End

            // Lab06 Start
            SampleData.InitializeFortunesAsync(app.ApplicationServices).Wait();
            // Lab06 End
        }
    }
}
