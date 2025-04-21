namespace MultithreadedServerSim.Interfaces;

internal interface IRequestHandlerResolver
{
    IRequestHandler Resolve(string path, CancellationToken cancellationToken = default);
    void RegisterHandler(string path, Func<IRequestHandler> requestHandlerCreator);
}
