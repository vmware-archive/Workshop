using CloudFoundry.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Web.Mvc;

namespace CloudFoundry.Controllers
{
    public class HomeController : Controller
    {
        private IOptionsSnapshot<AppConfiguration> AppConfiguration { get; set; }
        private IOptionsSnapshot<SubSectionConfiguration> SubSectionConfiguration { get; set; }
        private IOptions<CloudFoundryServicesOptions> CloudFoundryServices { get; set; }
        private IOptions<CloudFoundryApplicationOptions> CloudFoundryApplication { get; set; }
        private IConfigurationRoot Config { get; set; }

        public HomeController(IConfigurationRoot config,
            IOptions<CloudFoundryApplicationOptions> cloudAppOptions,
            IOptions<CloudFoundryServicesOptions> cloudServiceOptions,
            IOptionsSnapshot<AppConfiguration> appConfig,
            IOptionsSnapshot<SubSectionConfiguration> subsectionConfig)
        {
            Config = config;
            CloudFoundryApplication = cloudAppOptions;
            CloudFoundryServices = cloudServiceOptions;
            AppConfiguration = appConfig;
            SubSectionConfiguration = subsectionConfig;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CloudFoundryConfig()
        {
            return View(new CloudFoundryViewModel(CloudFoundryApplication.Value,CloudFoundryServices.Value));
        }

        public ActionResult AppConfig()
        {
            return View(new AppConfigViewModel(AppConfiguration.Value));
        }

        public ActionResult SubSectionConfig()
        {
            return View(new SubSectionConfigViewModel(SubSectionConfiguration.Value));
        }
        public ActionResult RawConfig()
        {
            var items = Config.AsEnumerable();
            return View(Config);
        }

        public ActionResult KillApp()
        {
            Console.WriteLine("Kaboom.");
            Environment.Exit(-1);
            return View();
        }
    }
}