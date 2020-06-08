using System;

namespace DotNetCli.Exceptions
{
    public class DotNetCliException : Exception
    {
        public DotNetCliException(string message) : base(message){}
    }
}