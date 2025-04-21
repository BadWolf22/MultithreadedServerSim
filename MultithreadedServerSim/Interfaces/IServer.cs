using MultithreadedServerSim.Contracts;

namespace MultithreadedServerSim.Interfaces;

internal interface IServer
{
    bool Request(Request request);
    void ShutdownGracefully();
}
