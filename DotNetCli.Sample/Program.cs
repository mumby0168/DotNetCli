using System;
using System.Threading.Tasks;
using DotNetCli.Attributes;
using DotNetCli.Interfaces;

namespace DotNetCli.Sample
{
    public class Example : ICommandHandler
    {
        [Command("Shows an async method saying hello!")]
        public async Task Hello()
        {
            await Task.Run(() => Console.WriteLine("Hello from an async method"));
        }

        [Command("Shows a method saying hello!")]
        public void Goodbye() => Console.WriteLine("Goodbye from a normal method");

        [Command("Shows a method with parameters saying hello!")]
        public void PrintName(string name) => Console.WriteLine("Hello: " + name);
    }
    
    
    class Program
    {
        static async Task Main(string[] args)
        {
            DotNetCli.Init();
            DotNetCli.RegisterHandler<Example>();
            await DotNetCli.RunAsync(args);
        }
    }
}