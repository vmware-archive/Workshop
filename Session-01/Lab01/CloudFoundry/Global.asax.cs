using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CloudFoundry
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string SUBSECTION = "subsection";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Build the applications configuration
            AppConfig.BuildConfiguration();

            // Create Autofac container builder
            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Adds IConfigurationRoot and adds IOption<CloudFoundryApplicationOptions>, IOption<CloudFoundryServicesOptions> to the Autofac container
            builder.RegisterConfiguration(AppConfig.Configuration);

            // Bind application configuration to AppConfiguration POCO and add IOption<AppConfiguration> to Autofac container
            builder.Configure<AppConfiguration>(AppConfig.Configuration);

            // Bind a section of the application configuration to SubSectionConfiguration POCO and add IOption<SubSectionConfiguration> to Autofac container
            builder.Configure<SubSectionConfiguration>(AppConfig.Configuration.GetSection(SUBSECTION));

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
