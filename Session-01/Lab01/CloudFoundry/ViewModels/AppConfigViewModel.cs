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