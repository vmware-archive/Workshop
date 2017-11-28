
// Lab01 - Lab04 Start

namespace CloudFoundry.ViewModels
{
    public class AppConfigViewModel
    {
        public AppConfigurationOptions AppConfig { get; }
        public AppConfigViewModel(AppConfigurationOptions appConfig)
        {
            AppConfig = appConfig ?? new AppConfigurationOptions();
        }
    }
}

// Lab01 - Lab04 End