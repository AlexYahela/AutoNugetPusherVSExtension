using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetProjExtension.Commands.Interfaces
{
    public interface IExecute: IDisposable
    {
        Task<string> execCMD(string command);

        void Initialize();

    }
}
