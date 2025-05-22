using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.RequestHandlers;

internal class DefaultRequestHandler : IRequestHandler
{
    public Response HandleRequest(dynamic details)
    {
        return new Response(200, details.body);
    }
}
