
// Lab01 - Lab04 Start

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
// Lab01 - Lab04 End