using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.RequestHandlers;

internal class DefaultRequestHandler : IRequestHandler
{
    public Response HandleRequest(Request request)
    {
        Thread.Sleep(5000);
        return new Response(200, request.Body);
    }
}
