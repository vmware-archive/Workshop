

namespace CloudFoundry.ViewModels
{
    public class SubSectionConfigViewModel
    {
        public SubSectionConfiguration SubSectionConfig { get; }
        public SubSectionConfigViewModel(SubSectionConfiguration sectionConfig)
        {
            SubSectionConfig = sectionConfig ?? new SubSectionConfiguration();
        }
    }
}