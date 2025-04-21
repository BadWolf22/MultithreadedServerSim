using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.RequestHandlers;

internal class ShutdownRequestHandler : IRequestHandler
{
    public Response HandleRequest(Request request)
    {
        return new Response(503, "The server is not handling requests right now, please try again soon.");
    }
}
