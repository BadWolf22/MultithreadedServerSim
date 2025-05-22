using MultithreadedServerSim.Contracts;

namespace MultithreadedServerSim.Interfaces;

internal interface IRequestHandlerResolver
{
    Func<Action?, Response> Resolve(Request request, CancellationToken cancellationToken = default);
}