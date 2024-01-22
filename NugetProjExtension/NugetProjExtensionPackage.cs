global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell.Interop;
using NugetProjExtension.Options;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace NugetProjExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(Microsoft.VisualStudio.Shell.Interop.UIContextGuids80.SolutionExists)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideOptionPage(typeof(OptionsProvider.GitlabOptionsOptions), "NugetProjExtension", "GitlabOptions", 0, 0, true, SupportsProfiles = true)]
    [ProvideOptionPage(typeof(OptionsProvider.GithubOptionsOptions), "NugetProjExtension", "GithubOptions", 0, 0, true, SupportsProfiles = true)]
    [ProvideOptionPage(typeof(OptionsProvider.OtherSourceOptions), "NugetProjExtension", "OtherSource", 0, 0, true, SupportsProfiles = true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.NugetProjExtensionString)]
    public sealed class NugetProjExtensionPackage : AsyncPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {          
            await this.RegisterCommandsAsync();        
        }

      

    }
}