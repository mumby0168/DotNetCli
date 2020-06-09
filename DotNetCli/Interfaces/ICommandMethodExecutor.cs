using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNetCli.Interfaces
{
    public interface ICommandMethodExecutor
    {
        MethodInfo ValidateType(Type type, string[] command);

        Task Execute(MethodInfo methodInfo, string[] command, object classInstance);
    }
}