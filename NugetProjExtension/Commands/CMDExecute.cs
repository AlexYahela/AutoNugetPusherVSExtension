using EnvDTE;
using NugetProjExtension.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetProjExtension.Commands
{
    public class CMDExecute : IExecute
    {
        private readonly System.Diagnostics.Process pro = new System.Diagnostics.Process();



        public async Task<string> execCMD(string command)
        {
            pro.Start();

            await pro.StandardInput.WriteLineAsync("chcp 437");
            await pro.StandardInput.WriteLineAsync("set DOTNET_CLI_UI_LANGUAGE=en");
            await pro.StandardInput.WriteLineAsync("echo off");
            //pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;         
            await pro.StandardInput.WriteLineAsync(command);

            

            await pro.StandardInput.WriteLineAsync("exit");
            pro.StandardInput.AutoFlush = true;
            //Get the output information of the cmd window

            pro.WaitForExit();
            string output = await pro.StandardOutput.ReadToEndAsync();
            //pro.WaitForExit();//Wait for the program to finish and exit the process
            //pro.Close();
            return output;
        }

        public void Initialize()
        {
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.CreateNoWindow = true;
        }

        public void Dispose()
        {
            pro.WaitForExit();//Wait for the program to finish and exit the process
            pro.Close();
        }
    }
}
