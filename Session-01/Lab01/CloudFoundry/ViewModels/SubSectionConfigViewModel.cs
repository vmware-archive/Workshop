using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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