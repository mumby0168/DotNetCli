using System;
using System.Threading.Tasks;
using Autofac;
using DotNetCli.Exceptions;
using DotNetCli.Interfaces;
using DotNetCli.Services;

namespace DotNetCli
{
    public static class DotNetCli
    {
        internal static IContainer Container { get; private set; }
        public static ContainerBuilder ContainerBuilder;
        public static void Init()
        {
            ContainerBuilder = new ContainerBuilder();
            ContainerBuilder.RegisterType<CommandExecutor>().As<ICommandExecutor>();
            ContainerBuilder.RegisterType<CommandMethodExecutor>().As<ICommandMethodExecutor>();
            ContainerBuilder.RegisterType<HandlerTypeFinder>().As<IHandlerTypeFinder>();
            ContainerBuilder.RegisterType<HelpGenerator>().As<IHelpGenerator>();
            //TODO Register internal services.
        }

        public static void RegisterHandler<T>() where T : ICommandHandler
        {
            ContainerBuilder.RegisterType<T>();
        }

        public static async Task RunAsync(string[] input)
        {
            if (Container is null)
            {
                Container = ContainerBuilder.Build();
            }
            
            var executor = Container.Resolve<ICommandExecutor>();

            try
            {
                await executor.Execute(input);
            }
            catch (DotNetCliException e)
            {
                Console.WriteLine("cli error: " + e.Message);
            }
        }
    }
}