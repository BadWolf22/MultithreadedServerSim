using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Helpers;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.Services;

internal class TicketService(ITicketRepository ticketRepository, TicketLock ticketLock) : ITicketService
{
    public HashSet<int> GetAvailableTicketIds()
    {
        HashSet<int> action() => ticketRepository.GetAvailableTicketIds();
        //return LockHelper.ExecuteWithTryMonitor(action, ticketLock);
        //return LockHelper.ExecuteWithMonitor(action, ticketLock);
        //return LockHelper.ExecuteWithLock(action, ticketLock);
        //return LockHelper.ExecuteWithMutex(action, nameof(ticketLock));
        return LockHelper.ExecuteWithReadLock(action, ticketLock);
        //return action();
    }

    public void BookTicket(int id, string userName)
    {
        void action() => ticketRepository.BookTicket(id, userName);
        //LockHelper.ExecuteWithTryMonitor(action, ticketLock);
        //LockHelper.ExecuteWithMonitor(action, ticketLock);
        //LockHelper.ExecuteWithLock(action, ticketLock);
        //LockHelper.ExecuteWithMutex(action, nameof(ticketLock));
        LockHelper.ExecuteWithWriteLock(action, ticketLock);
        //action();
    }

    public void ReleaseTicket(int id)
    {
        void action() => ticketRepository.ReleaseTicket(id);
        //LockHelper.ExecuteWithTryMonitor(action, ticketLock);
        //LockHelper.ExecuteWithMonitor(action, ticketLock);
        //LockHelper.ExecuteWithLock(action, ticketLock);
        //LockHelper.ExecuteWithMutex(action, nameof(ticketLock));
        LockHelper.ExecuteWithWriteLock(action, ticketLock);
        //action();
    }
}
