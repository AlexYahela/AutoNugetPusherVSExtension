using NugetProjExtension.Options;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NugetProjExtension
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.OtherSourceOptions), "NugetProjExtension", "OtherSource", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class OtherSourceOptions : BaseOptionPage<OtherSource> { }
    }

    public class OtherSource : SourceOptions<OtherSource>
    {
        public OtherSource() 
        {
            SpecificName = "OtherSource";
        }
    }
}
