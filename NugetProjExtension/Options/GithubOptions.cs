using System.ComponentModel;
using NugetProjExtension.Options;
using System.Runtime.InteropServices;

namespace NugetProjExtension
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.GithubOptionsOptions), "NugetProjExtension.Options", "GithubOptions", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class GithubOptionsOptions : BaseOptionPage<GithubOptions> { }
    }

    public class GithubOptions : SourceOptions<GithubOptions>
    {
        public GithubOptions() 
        {
            IsUseSpecificPath = true;
            SpecificName = "Github";
            SpecificPath = "https://nuget.pkg.github.com/DefinitelyNotRadar/index.json";
        }
    }
}
