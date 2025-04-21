using MultithreadedServerSim;
using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Helpers;

internal class Program
{
    private static void Main(string[] args)
    {
        var resolver = new RequestHandlerResolver();
        var server = new Server(resolver);

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
            ConsoleWrapper.WriteLine("");
            ConsoleWrapper.WriteLine("");
        }
    }
}
