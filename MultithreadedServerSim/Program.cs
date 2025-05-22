using Autofac;
using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Interfaces;

internal class Program
{
    internal static IContainer? _container;

    private static void Main(string[] args)
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<MyModule>();
        _container = builder.Build();
        var server = _container.Resolve<IServer>();

        while (true)
        {
            ConsoleWrapper.Write("Path: ");
            var path = ConsoleWrapper.ReadLine();
            if (path.Trim().Equals("shutdown", StringComparison.CurrentCultureIgnoreCase))
            {
                server.ShutdownGracefully();
                break;
            }

            ConsoleWrapper.Write("Body: ");
            var body = ConsoleWrapper.ReadLine();
            if (body == null)
                continue;

            var top = ConsoleWrapper.GetTop();
            var request = new Request(path, body, response => ConsoleWrapper.WriteAtHeight(response.Body, top));
            if (!server.Request(request))
            {
                ConsoleWrapper.WriteLine("Request Rejected");
                continue;
            }
            ConsoleWrapper.WriteLine("Waiting...");
            ConsoleWrapper.WriteLine("");
        }
    }
}
