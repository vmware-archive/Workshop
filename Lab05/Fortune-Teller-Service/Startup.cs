using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Fortune_Teller_Service.Models;


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

            // Lab05 Start
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<FortuneContext>(
                    options => options.UseInMemoryDatabase("Fortunes"));
            // Lab05 End

            // Lab05 Start
            services.AddScoped<IFortuneRepository, FortuneRepository>();
            // Lab05 End

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
        }
    }
}
