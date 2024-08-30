using Microsoft.VisualStudio.Text;
using System.Diagnostics;
using System.Xml.Linq;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.Shell.Interop;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NugetProjExtension.Options;
using NugetProjExtension.Commands.Interfaces;
using NugetProjExtension.Commands;

namespace NugetProjExtension
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {

        private readonly string[] endingPathCollection = new string[]
        {
            "\\",
            "\\Debug\\",
            "\\Release\\",
            "\\arm64\\Debug\\",
            "\\arm64\\Release\\",
            "\\x86\\Debug\\",
            "\\x86\\Release\\",
            "\\x64\\Debug\\",
            "\\x64\\Release\\",
        };

        private IVsOutputWindowPane outputWindowPane;
        private IExecute execute;




        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            execute = new CMDExecute();
            execute.Initialize();


            await Package.JoinableTaskFactory.SwitchToMainThreadAsync();

            var paneGuid = Guid.NewGuid();
            var outputWindow = (IVsOutputWindow)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsOutputWindow));
            outputWindow.CreatePane(ref paneGuid, "My Window", 1, 1);
            outputWindow.GetPane(ref paneGuid, out var outputPane);

            outputWindowPane = outputPane;

            outputWindowPane.Activate();
            await AddStringToOutputAsync("Start Pushing...\n");

            //var generalSettings = await General1.GetLiveInstanceAsync();
            var project = await VS.Solutions.GetActiveProjectAsync();


            //Find nuget package version
            XDocument xmldoc = XDocument.Load(project.FullPath);
            string pckgVersion = xmldoc.Document.Element("Project").Element("PropertyGroup").Element("Version").Value;

            //Get list of existing nuget sources
            var nugetSourceForGithubCommandResult = await execute.execCMD("dotnet nuget list source");
            var splitedString = nugetSourceForGithubCommandResult.Split('\n');

            var packageName = project.Name + "." + pckgVersion + ".nupkg";
            var fullPathToPackage = FindPathToNugetPackage(project.FullPath, packageName);

            if (fullPathToPackage == null)
            {
                await VS.StatusBar.ShowMessageAsync("Nuget Package not found.");
                return;
            }

            string nupckPushCommandWithoutSource = "dotnet nuget push " + fullPathToPackage + " --source ";

            await PrepareSourceAndPushAsync(splitedString, GithubOptions.Instance, nupckPushCommandWithoutSource);
            await PrepareSourceAndPushAsync(splitedString, GitlabOptions.Instance, nupckPushCommandWithoutSource);
            await PrepareSourceAndPushAsync(splitedString, OtherSource.Instance, nupckPushCommandWithoutSource);


            execute.Dispose();
        }

        private async Task PrepareSourceAndPushAsync<T>(string[] splitedString, SourceOptions<T> generalSettings, string nupckPushCommandWithoutSource) where T : BaseOptionModel<T>, new()
        {
            if (!generalSettings.IsUseSpecificPath) return;

            var nameForConcretePath = FindSourceName(splitedString, generalSettings.SpecificPath);
            if (nameForConcretePath != null)
            {
                if (generalSettings.UserName.Trim() != string.Empty && generalSettings.PasswordToken.Trim() != string.Empty)
                {
                    var answer = await execute.execCMD("dotnet nuget update source " +
                        nameForConcretePath +
                        " --username " + generalSettings.UserName +
                        " --password " + generalSettings.PasswordToken);
                    await AddStringToOutputAsync("Password and Username updated to " + nameForConcretePath);
                    await AddStringToOutputAsync(answer);
                }               
            }
            else
            {
                var answer = await execute.execCMD("dotnet nuget add source " + generalSettings.SpecificPath +
                    " --name " + generalSettings.SpecificName +
                    " --username " + generalSettings.UserName +
                    " --password " + generalSettings.PasswordToken);

                nameForConcretePath = generalSettings.SpecificPath;

               await AddStringToOutputAsync("Added new nuget source: " + nameForConcretePath);
               await AddStringToOutputAsync(answer);
            }

            await PushToSourceAsync(nameForConcretePath, nupckPushCommandWithoutSource);
        }

        private string FindSourceName(string[] splitedString, string sourceIdentifaer)
        {

            for (int i = 1; i < splitedString.Length; i++)
            {
                var sourcePath = splitedString[i];

                if (sourcePath.Contains(sourceIdentifaer))
                {
                    var sourceWithStatus = splitedString[i - 1].TrimStart().Substring(3);
                    int openBracketIndex = sourceWithStatus.IndexOf('[');
                    var NameSource = sourceWithStatus.Substring(0, openBracketIndex).TrimEnd();
                    return NameSource;
                }
            }

            return null;
        }

        private async Task PushToSourceAsync(string sourceName, string beforeSource)
        {
            var cmd = beforeSource + sourceName;
            var answer = await execute.execCMD(cmd);

           await AddStringToOutputAsync("Execute command: " + cmd);

            if (answer.ToLower().Contains("error"))
            {                
                await VS.MessageBox.ShowErrorAsync("Error Nuget Push. Command: " + 
                    cmd + "\n" +
                    answer.Substring(0, answer.IndexOf("error")));
               await AddStringToOutputAsync(answer);
            }
            else
            {
               await AddStringToOutputAsync("Success pushing to " + sourceName);
            }
        }


        private string FindPathToNugetPackage(string fullPathToProject, string packageName)
        {
            var pathToBin = fullPathToProject.Substring(0, fullPathToProject.LastIndexOf('\\')) + "\\bin";
         

            foreach (var path in endingPathCollection)
            {
                var fullpath = pathToBin + path + packageName;
                if (File.Exists(fullpath))
                {
                    return fullpath;
                }
            }

            return null;
        }

        private async Task AddStringToOutputAsync(string outputString)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            outputWindowPane.OutputString(outputString + "\n");
            await TaskScheduler.Default;
        }
     
    }
}
