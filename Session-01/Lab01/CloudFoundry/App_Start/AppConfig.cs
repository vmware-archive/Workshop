using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using PA = Microsoft.Extensions.PlatformAbstractions;

namespace CloudFoundry
{
    public static class AppConfig
    {
        // Application Configuration values
        public static IConfigurationRoot Configuration { get; private set; }

        public static void BuildConfiguration()
        {
            var env = new HostingEnvironment();

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)

                // Add json files as configuration source
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)

                // Parse and add VCAP_APPLICATION and VCAP_SERVICES as configuration data
                .AddCloudFoundry()

                // Add environment variables as configuration source
                .AddEnvironmentVariables();

            // Build the applications configuration and save
            Configuration = builder.Build();
        }

        /// <summary>
        /// Register IOptions infrastructure and bind VCAP_APPLICATION configuration data to CloudFoundryApplicationOptions
        /// and bind VCAP_SERVICES configuration data to CloudFoundryServicesOptions then add IOption<CloudFoundryApplicationOptions>,
        /// IOption<CloudFoundryServicesOptions>, and IOption<IConfigurationRoot> to container
        /// </summary>
        /// <param name="container">autofac container builder</param>
        /// <param name="config">configuration data to use</param>
        public static void RegisterConfiguration(this ContainerBuilder container, IConfigurationRoot config)
        {
            // Register IOptions infrastructure with the Autofac container
            container.RegisterOptions();

            // Bind VCAP_APPLICATION configuration data to CloudFoundryApplicationOptions and add IOption<CloudFoundryApplicationOptions> to container
            container.Configure<CloudFoundryApplicationOptions>(config);

            // Bind VCAP_SERVICES configuration data to CloudFoundryServicesOptions and add IOption<CloudFoundryServicesOptions> to container
            container.Configure<CloudFoundryServicesOptions>(config);

            // Add IOption<IConfigurationRoot> to container
            container.RegisterInstance(config).As<IConfigurationRoot>().SingleInstance();
        }

        /// <summary>
        /// Register IOptions infrastructure with the Autofac container
        /// </summary>
        /// <param name="container">autofac container builder instance</param>
        public static void RegisterOptions(this ContainerBuilder container)
        {
            container.RegisterGeneric(typeof(OptionsManager<>)).As(typeof(IOptions<>)).SingleInstance();
            container.RegisterGeneric(typeof(OptionsMonitor<>)).As(typeof(IOptionsMonitor<>)).SingleInstance();
            container.RegisterGeneric(typeof(OptionsSnapshot<>)).As(typeof(IOptionsSnapshot<>)).InstancePerRequest();
        }

        /// <summary>
        /// Bind configuration to POCO and add IOption<POCO> to Autofac container
        /// </summary>
        /// <param name="container">autofac container builder instance</param>
        /// <param name="config">configuration</param>
        public static void Configure<POCO>(this ContainerBuilder container, IConfiguration config) where POCO : class
        {
            container.RegisterInstance(new ConfigurationChangeTokenSource<POCO>(config)).As<IOptionsChangeTokenSource<POCO>>().SingleInstance();
            container.RegisterInstance(new ConfigureFromConfigurationOptions<POCO>(config)).As<IConfigureOptions<POCO>>().SingleInstance();
        }
    }

    public class HostingEnvironment : IHostingEnvironment
    {
        public const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
        public HostingEnvironment()
        {
            EnvironmentName = System.Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT) ?? "development";
            ApplicationName = PA.PlatformServices.Default.Application.ApplicationName;
            ContentRootPath = PA.PlatformServices.Default.Application.ApplicationBasePath;
        }

        public string ApplicationName { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }

        public string ContentRootPath { get; set; }

        public string EnvironmentName { get; set; }

        public IFileProvider WebRootFileProvider { get; set; }

        public string WebRootPath { get; set; }

        IFileProvider IHostingEnvironment.WebRootFileProvider { get; set; }
    }
}