using System.Threading.Tasks;
using Autofac;
using DotNetCli.Interfaces;

namespace DotNetCli.Services
{
    public class CommandExecutor : ICommandExecutor
    {
        private const string Help = "Help";
        private const string Exit = "Exit";
        private readonly IHandlerTypeFinder _handlerTypeFinder;
        private readonly IHelpGenerator _helpGenerator;
        private readonly ICommandMethodExecutor _commandMethodExecutor;

        public CommandExecutor(IHandlerTypeFinder handlerTypeFinder, IHelpGenerator helpGenerator, ICommandMethodExecutor commandMethodExecutor)
        {
            _handlerTypeFinder = handlerTypeFinder;
            _helpGenerator = helpGenerator;
            _commandMethodExecutor = commandMethodExecutor;
        }
        public async Task Execute(string userInput)
        {
            if (userInput == Help)
            {
                _helpGenerator.GenerateHelp();
                return;
            }

            if (userInput == Exit)
            {
                return;
            }
            
            var command =  userInput.Split(' ');
            var type = _handlerTypeFinder.ResolveType(userInput);
            var instance = DotNetCli.Container.Resolve(type);
            var methodInfo = _commandMethodExecutor.ValidateType(type, command);
            await _commandMethodExecutor.Execute(methodInfo, command, instance);
        }
    }
}