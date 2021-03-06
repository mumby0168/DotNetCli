using System;
using System.Linq;
using System.Reflection;
using DotNetCli.Exceptions;
using DotNetCli.Interfaces;

namespace DotNetCli.Services
{
    public class HandlerTypeFinder : IHandlerTypeFinder
    {
        public Type ResolveType(string[] input)
        {

            if (input.Length < 2)
                throw new DotNetCliException(
                    "Most commands have two parts at minimum. Did you mean \"Exit\"? Please try again.");

            var classInfo = Assembly.GetEntryAssembly().GetTypes().FirstOrDefault(t => t.Name == input[0] && typeof(ICommandHandler).IsAssignableFrom(t));
            if (classInfo == null)
                throw new DotNetCliException(
                    $"Couldn't resolve the first part of the command path \"{input[0]}\". Please try again.");

            return classInfo;
        }
    }
}