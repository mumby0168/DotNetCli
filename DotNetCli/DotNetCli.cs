﻿using System;
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
        private static ContainerBuilder _containerBuilder;
        public static void Init()
        {
            _containerBuilder = new ContainerBuilder();
            _containerBuilder.RegisterType<CommandExecutor>().As<ICommandExecutor>();
            _containerBuilder.RegisterType<CommandMethodExecutor>().As<ICommandMethodExecutor>();
            _containerBuilder.RegisterType<HandlerTypeFinder>().As<IHandlerTypeFinder>();
            _containerBuilder.RegisterType<HelpGenerator>().As<IHelpGenerator>();
            //TODO Register internal services.
        }

        public static void RegisterHandler<T>() where T : ICommandHandler
        {
            _containerBuilder.RegisterType<T>();
        }

        public static async Task RunAsync(string[] input)
        {
            if (Container is null)
            {
                Container = _containerBuilder.Build();
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