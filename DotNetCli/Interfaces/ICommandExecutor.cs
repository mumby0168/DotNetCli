using System.Threading.Tasks;

namespace DotNetCli.Interfaces
{
    public interface ICommandExecutor
    {
        Task Execute(string userInput);
    }
}