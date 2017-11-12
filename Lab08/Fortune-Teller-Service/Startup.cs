using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Fortune_Teller_Service.Models;

// Lab07 Start
using Pivotal.Discovery.Client;
// Lab07 End

// Lab08 Start
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
// Lab08 End

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

            // Lab08 add
            if (Environment.IsDevelopment())
            {
                // Lab05 Start
                services.AddEntityFrameworkInMemoryDatabase().AddDbContext<FortuneContext>(
                options => options.UseInMemoryDatabase("Fortunes"), ServiceLifetime.Singleton);
                // Lab05 End
            }
            else
            {
                // Lab08 add
                services.AddDbContext<FortuneContext>(options => options.UseMySql(Configuration));
            }

            // Lab05 Start
            services.AddSingleton<IFortuneRepository, FortuneRepository>();
            // Lab05 End

            // Lab07 Start
            services.AddDiscoveryClient(Configuration);
            // Lab07 End

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Lab05 Start
            SampleData.InitializeFortunesAsync(app.ApplicationServices).Wait();
            // Lab05 End

            // Lab07 Start
            app.UseDiscoveryClient();
            // Lab07 End
        }
    }
}
