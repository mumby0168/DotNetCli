using System;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetCli.Attributes;
using DotNetCli.Interfaces;

namespace DotNetCli.Services
{
    public class HelpGenerator : IHelpGenerator
    {
        public void GenerateHelp()
        {
            {
                var commandHandlers = Assembly.GetEntryAssembly().GetTypes().Where(t => typeof(ICommandHandler).IsAssignableFrom(t));
                foreach (var commandHandler in commandHandlers)
                {
                    var commandMethods = commandHandler.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.GetCustomAttribute<CommandAttribute>() != null);

                    var methodInfos = commandMethods.ToList();
                    
                    if (methodInfos.Any())
                        Console.WriteLine(commandHandler.Name);    

                    foreach (var commandMethod in methodInfos)
                    {
                        var parameters = commandMethod.GetParameters();
                        var parametersString = new StringBuilder();
                        foreach (var parameterInfo in parameters)
                        {
                            parametersString.Append(parameterInfo.Name + ",");
                        }
                        
                        Console.WriteLine($"\t{commandHandler.Name} {commandMethod.Name} [{parametersString}] - {commandMethod.GetCustomAttribute<CommandAttribute>().Description}");
                    }
                }

                Console.WriteLine("Exit - Exit the client application.");
            }
        }
    }
}