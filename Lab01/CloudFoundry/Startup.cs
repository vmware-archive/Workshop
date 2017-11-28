using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace CloudFoundry
{
    public class Startup
    {
        private const string SUBSECTION = "subsection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Lab01 - Lab04 Start

            // Add Steeltoe CloudFoundry Options to service container
            services.ConfigureCloudFoundryOptions(Configuration);

            // Add IConfiguration to container
            services.AddSingleton<IConfiguration>(Configuration);

            // Bind application configuration to AppConfiguration POCO and add IOption<AppConfiguration> to Autofac container
            services.Configure<AppConfigurationOptions>(Configuration);

            // Bind a section of the application configuration to SubSectionConfiguration POCO and add IOption<SubSectionConfiguration> to Autofac container
            services.Configure<SubSectionConfigurationOptions>(Configuration.GetSection(SUBSECTION));

            // Lab01 - Lab04 End

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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
