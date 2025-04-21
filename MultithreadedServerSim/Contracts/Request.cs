namespace MultithreadedServerSim.Contracts;

internal record Request(string Path, string Body, Action<Response> Respond);
