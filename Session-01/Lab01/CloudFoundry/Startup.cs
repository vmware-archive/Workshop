
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace CloudFoundry
{
    public class Startup
    {
        private const string SUBSECTION = "subsection";
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddCloudFoundry()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Bind VCAP_APPLICATION configuration data to CloudFoundryApplicationOptions and add IOption<CloudFoundryApplicationOptions> to container
            services.Configure<CloudFoundryApplicationOptions>(Configuration);

            // Bind VCAP_SERVICES configuration data to CloudFoundryServicesOptions and add IOption<CloudFoundryServicesOptions> to container
            services.Configure<CloudFoundryServicesOptions>(Configuration);

            // Add IConfigurationRoot to container
            services.AddSingleton<IConfigurationRoot>(Configuration);

            // Bind application configuration to AppConfiguration POCO and add IOption<AppConfiguration> to Autofac container
            services.Configure<AppConfiguration>(Configuration);

            // Bind a section of the application configuration to SubSectionConfiguration POCO and add IOption<SubSectionConfiguration> to Autofac container
            services.Configure<SubSectionConfiguration>(Configuration.GetSection(SUBSECTION));

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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
