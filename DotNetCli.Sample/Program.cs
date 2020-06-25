using System;
using System.Threading.Tasks;
using Autofac;
using DotNetCli.Attributes;
using DotNetCli.Exceptions;
using DotNetCli.Interfaces;

namespace DotNetCli.Sample
{
    public interface ISomeService
    {
        void Hello() => Console.WriteLine("Hello there");
    }

    public class SomeService : ISomeService
    {
        
    }
    
    
    public class Example : ICommandHandler
    {
        private readonly ISomeService _someService;

        public Example(ISomeService someService)
        {
            _someService = someService;
        }
        [Command("Shows an async method saying hello!")]
        public async Task Hello()
        {
            await Task.Run(() => Console.WriteLine("Hello from an async method"));
        }

        [Command("Shows a method saying hello!")]
        public void Goodbye() => Console.WriteLine("Goodbye from a normal method");

        [Command("Shows a method with parameters saying hello!")]
        public void PrintName(string name) => Console.WriteLine("Hello: " + name);

        [Command("Invokes a serivce injected")]
        public void ServiceHello() => _someService.Hello();
    }
    
    
    class Program
    {
        static async Task Main(string[] args)
        {
            DotNetCli.Init();
            DotNetCli.ContainerBuilder.RegisterType<SomeService>().As<ISomeService>();
            DotNetCli.RegisterHandler<Example>();
            await DotNetCli.RunAsync(args);
        }
    }
}