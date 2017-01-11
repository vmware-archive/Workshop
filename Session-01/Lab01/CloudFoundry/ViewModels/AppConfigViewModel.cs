using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudFoundry.ViewModels
{
    public class AppConfigViewModel
    {
        public AppConfiguration AppConfig { get; }
        public AppConfigViewModel(AppConfiguration appConfig)
        {
            AppConfig = appConfig ?? new AppConfiguration();
        }
    }
}