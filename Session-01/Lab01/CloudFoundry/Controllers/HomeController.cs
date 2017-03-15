
using System;
using CloudFoundry.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;

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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CloudFoundryConfig()
        {
            return View(new CloudFoundryViewModel(CloudFoundryApplication.Value,CloudFoundryServices.Value));
        }

        public IActionResult AppConfig()
        {
            return View(new AppConfigViewModel(AppConfiguration.Value));
        }

        public IActionResult SubSectionConfig()
        {
            return View(new SubSectionConfigViewModel(SubSectionConfiguration.Value));
        }

        public IActionResult RawConfig()
        {
            var items = Config.AsEnumerable();
            return View(Config);
        }
        public IActionResult KillApp()
        {
            Console.WriteLine("Kaboom.");
            Environment.Exit(-1);
            return View();
        }
    }
}
