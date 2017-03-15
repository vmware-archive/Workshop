using Steeltoe.Extensions.Configuration.CloudFoundry;


namespace CloudFoundry.ViewModels
{
    public class CloudFoundryViewModel
    {
        public CloudFoundryViewModel(CloudFoundryApplicationOptions appOptions, CloudFoundryServicesOptions servOptions)
        {
            CloudFoundryServices = servOptions ?? new CloudFoundryServicesOptions();
            CloudFoundryApplication = appOptions ?? new CloudFoundryApplicationOptions();
        }
        public CloudFoundryServicesOptions CloudFoundryServices { get; }
        public CloudFoundryApplicationOptions CloudFoundryApplication { get; }
    }
}