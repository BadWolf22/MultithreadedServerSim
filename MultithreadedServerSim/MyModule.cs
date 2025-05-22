using Autofac;
using MultithreadedServerSim;
using MultithreadedServerSim.Attributes;
using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Helpers;
using MultithreadedServerSim.Interfaces;
using MultithreadedServerSim.Repos;
using MultithreadedServerSim.Services;
using System.Reflection;

internal class MyModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Server>().As<IServer>().SingleInstance();
        builder.RegisterType<InMemoryTicketRepository>().As<ITicketRepository>().SingleInstance();
        builder.RegisterInstance(new TicketLock()).SingleInstance();

        builder.RegisterType<TicketService>().As<ITicketService>();
        builder.RegisterType<RequestHandlerResolver>().As<IRequestHandlerResolver>();

        var classes = Assembly.GetAssembly(typeof(Program))!.GetTypes().Where(type => type.IsClass);
        var requestHandlers = classes.Where(type => type.GetCustomAttribute<Controller>(inherit: true) is not null);
        foreach (var handler in requestHandlers)
        {
            var controller = handler.GetCustomAttribute<Controller>()!.Path;
            var routes = handler.GetCustomAttributes<Route>();
            foreach (var route in routes)
            {
                builder.RegisterType(handler).As<IRequestHandler>().WithMetadata(nameof(Controller), controller).WithMetadata(nameof(Route), route.Path);
            }
        }
    }
}
