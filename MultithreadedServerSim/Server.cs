using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim;

internal class Server : IServer
{
    private readonly IRequestHandlerResolver _requestHandlerResolver;
    private readonly Queue<Request> _requestQueue;
    private readonly Thread _requestProcessor;
    private readonly Dictionary<Guid, Thread> _requestsInProgress;
    private readonly CancellationTokenSource _processingActive;

    public Server(IRequestHandlerResolver requestHandlerResolver)
    {
        _requestHandlerResolver = requestHandlerResolver;
        _requestQueue = [];
        _requestsInProgress = [];
        _processingActive = new CancellationTokenSource();
        _requestProcessor = new Thread(() =>
        {
            while (true)
            {
                var didProcess = ProcessRequestInNewThread();
                if (_processingActive.IsCancellationRequested && !didProcess)
                    break;
            }
        });
        _requestProcessor.Start();
    }

    public bool Request(Request request)
    {
        if (_processingActive.IsCancellationRequested)
            return false;

        _requestQueue.Enqueue(request);
        return true;
    }

    public void ShutdownGracefully()
    {
        ConsoleWrapper.WriteLine("SHUTDOWN INITIATED");
        _processingActive.Cancel();
        _requestProcessor.Join();
    }

    private bool ProcessRequestInNewThread()
    {
        if (!_requestQueue.TryDequeue(out var request))
            return false;

        var requestId = Guid.NewGuid();
        var handler = _requestHandlerResolver.Resolve(request.Path, _processingActive.Token);

        var requestThread = new Thread(() =>
        {
            var response = handler.HandleRequest(request);
            _requestsInProgress.Remove(requestId);
            request.Respond(response);
        });
        _requestsInProgress[requestId] = requestThread;
        requestThread.Start();

        return true;
    }
}