# Easy DotNet Cli Library

## Getting Started

The first thing to do is create a console application in either .NET Core or .NET Framework.

Then download and add the latest version of the nuget package ```EasyDotNetCliLibrary```.

## Setup 

A console application by default provides ```string[] args``` as a parameter to main. This is a good starting point to pass to the DotNetCli. A standard setup could look something like provided in the sample application:

```csharp
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
```

### Handlers Concept
The idea of a handler is what forms the function that your cli will support as shown above they can be registered into the built in AutoFac Container and resolved via dependancy injection (more on this later). The handler used in the sample application is shown below:

```csharp
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
```

All handlers must implement the marker interface ```ICommandHandler``` as this is how the framework picks them up. Methods can then be defined on this class marked with the ```[Command()]``` attribute. This takes an optional description that can be displayed to a user when they ask for the help section of the cli (again more on this later). The methods can also take parameters which can be passed to the function from a simple cli call. The library supprts both async and non async methods making it fit mutliple use cases. This also shows an example of using a handler which has a dependancy here ```ISomeService``` is defined by a user of the library and then registered by getting access to the AutoFac container which underpins this library. This can allow your handlers to even be unit testable!

## Using The Cli

In order to use the cli you can use the ```dotnet run``` command for .net core or run the exectuable build for framework to pass arguments to the program.

Navigating to the directory in which the project is contained we can execute our handlers examples of these are shown below:

### Hello()
Execute: ```dotnet run -- Example Hello```

Result: ```Hello from an async method```

### Goodbye()
Execute: ```dotnet run -- Example Goodbye```

Result: ```Goodbye from a normal method```

### PrintName(name)
Execute: ```dotnet run -- Example PrintName Joe```

Result: ```Hello: Joe```

### ServiceHello()
Execute: ```dotnet run -- Example ServiceHello```

Result: ```Hello there```

## The Help Feature
The system uses the class, method and parameter names along with the custom description to generate a help function on the cli the one generated from the sample application can be seen below by running: ```dotnet run -- Help```:

```
Example
	Example Hello [] - Shows an async method saying hello!
	Example Goodbye [] - Shows a method saying hello!
	Example PrintName [name,] - Shows a method with parameters saying hello!
	Example ServiceHello [] - Invokes a serivce injected
Exit - Exit the client application.
```

## Error Handling
In the case a command cannot be matched such as by executing ```dotnet run -- Not Here``` the following resposne is returned:
```
cli error: Couldn't resolve the first part of the command path "Not". Please try again.
```

All the methods executing are covered by a ```try``` ```catch``` handling any unhandled exceptions gracefully.


