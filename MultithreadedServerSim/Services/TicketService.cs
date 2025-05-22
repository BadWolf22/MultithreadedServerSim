using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Helpers;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.Services;

internal class TicketService(ITicketRepository ticketRepository, TicketLock ticketLock) : ITicketService
{
    public HashSet<int> GetAvailableTicketIds()
    {
        //return LockHelper.ExecuteWithTryMonitor(ticketRepository.GetAvailableTicketIds, ticketLock);
        //return LockHelper.ExecuteWithMonitor(ticketRepository.GetAvailableTicketIds, ticketLock);
        //return LockHelper.ExecuteWithLock(ticketRepository.GetAvailableTicketIds, ticketLock);
        return LockHelper.ExecuteWithMutex(ticketRepository.GetAvailableTicketIds, nameof(ticketLock));
    }

    public void BookTicket(int id, string userName)
    {
        //LockHelper.ExecuteWithTryMonitor(() => ticketRepository.BookTicket(id, userName), ticketLock);
        //LockHelper.ExecuteWithMonitor(() => ticketRepository.BookTicket(id, userName), ticketLock);
        //LockHelper.ExecuteWithLock(() => ticketRepository.BookTicket(id, userName), ticketLock);
        LockHelper.ExecuteWithMutex(() => ticketRepository.BookTicket(id, userName), nameof(ticketLock));
    }

    public void ReleaseTicket(int id)
    {
        //LockHelper.ExecuteWithTryMonitor(() => ticketRepository.ReleaseTicket(id), ticketLock);
        //LockHelper.ExecuteWithMonitor(() => ticketRepository.ReleaseTicket(id), ticketLock);
        //LockHelper.ExecuteWithLock(() => ticketRepository.ReleaseTicket(id), ticketLock);
        LockHelper.ExecuteWithMutex(() => ticketRepository.ReleaseTicket(id), nameof(ticketLock));
    }
}
