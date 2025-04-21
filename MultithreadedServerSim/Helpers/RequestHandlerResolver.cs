using MultithreadedServerSim.Interfaces;
using MultithreadedServerSim.RequestHandlers;

namespace MultithreadedServerSim.Helpers;

internal class RequestHandlerResolver : IRequestHandlerResolver
{
    private readonly Dictionary<string, Func<IRequestHandler>> _registeredEventHandlers = [];

    public IRequestHandler Resolve(string path, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return new ShutdownRequestHandler();

        var resolvedHandlerConstructor = _registeredEventHandlers.GetValueOrDefault(path);
        if (resolvedHandlerConstructor == null)
        {
            var slashIndex = path.IndexOf('/');
            if (slashIndex != -1)
                resolvedHandlerConstructor = _registeredEventHandlers.GetValueOrDefault(path[..slashIndex]);
        }
        resolvedHandlerConstructor ??= () => new DefaultRequestHandler();

        return resolvedHandlerConstructor();
    }

    public void RegisterHandler(string path, Func<IRequestHandler> requestHandlerCreator)
    {
        _registeredEventHandlers[path] = requestHandlerCreator;
    }
}
