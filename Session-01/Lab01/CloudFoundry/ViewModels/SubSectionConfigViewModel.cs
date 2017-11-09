

namespace CloudFoundry.ViewModels
{
    public class SubSectionConfigViewModel
    {
        public SubSectionConfigurationOptions SubSectionConfig { get; }
        public SubSectionConfigViewModel(SubSectionConfigurationOptions sectionConfig)
        {
            SubSectionConfig = sectionConfig ?? new SubSectionConfigurationOptions();
        }
    }
}