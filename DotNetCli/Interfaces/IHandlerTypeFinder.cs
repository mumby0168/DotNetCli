using System;

namespace DotNetCli.Interfaces
{
    public interface IHandlerTypeFinder
    {
        Type ResolveType(string[] input);
    }
}