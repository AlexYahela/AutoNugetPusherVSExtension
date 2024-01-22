using NugetProjExtension.Options;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NugetProjExtension
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.GitlabOptionsOptions), "NugetProjExtension", "GitlabOptions", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class GitlabOptionsOptions : BaseOptionPage<GitlabOptions> { }
    }

    public class GitlabOptions : SourceOptions<GitlabOptions>
    {
        public GitlabOptions() 
        {
            SpecificName = "Gitlab";
        }
        
    }
}
