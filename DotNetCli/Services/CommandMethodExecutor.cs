using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DotNetCli.Attributes;
using DotNetCli.Exceptions;
using DotNetCli.Extensions;
using DotNetCli.Interfaces;

namespace DotNetCli.Services
{
    public class CommandMethodExecutor : ICommandMethodExecutor
    {

        public MethodInfo ValidateType(Type type, string[] command)
        {
            var methodInfo = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.GetCustomAttribute<CommandAttribute>() != null).FirstOrDefault(m => m.Name == command[1]);
            if (methodInfo == null)
            {
                throw new DotNetCliException($"{command[0]} has no command \"{command[1]}\". Please try again or type \"Help\" to display a list of available commands.");
            }
            
            if (command.Length != (methodInfo.GetParameters().Length + 2))
            {
                throw new DotNetCliException($"Expected {methodInfo.GetParameters().Length} parameters. But {command.Length - 2} were given. Please try again.");
            }

            return methodInfo;
        }
        

        public async Task Execute(MethodInfo methodInfo, string[] command, object classInstance)
        {
            var parameters = new List<object>();
            foreach (var commandParameter in command.Skip(2).Take(command.Length - 2))
                parameters.Add((object)commandParameter);

            if (methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) as AsyncStateMachineAttribute != null)
            {
                try
                {
                    await methodInfo.InvokeAsync(classInstance, parameters.ToArray());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
            else
            {
                try
                {
                    methodInfo.Invoke(classInstance, parameters.ToArray());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
        }
    }
}