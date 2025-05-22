using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.Services;

internal class TicketService(ITicketRepository ticketRepository, TicketLock ticketLock) : ITicketService
{
    public HashSet<int> GetAvailableTicketIds()
    {
        return ExecuteWithLock(ticketRepository.GetAvailableTicketIds);
    }

    public void BookTicket(int id, string userName)
    {
        ExecuteWithLock(() => ticketRepository.BookTicket(id, userName));
    }

    public void ReleaseTicket(int id)
    {
        ExecuteWithLock(() => ticketRepository.ReleaseTicket(id));
    }

    private void ExecuteWithLock(Action action)
    {
        ExecuteWithLock(() => { action.Invoke(); return true; });
    }

    private TReturn ExecuteWithLock<TReturn>(Func<TReturn> action)
    {
        while (true)
        {
            if (!Monitor.TryEnter(ticketLock, 200))
                continue;
            try
            {
                return action.Invoke();
            }
            finally
            {
                Monitor.Exit(ticketLock);
            }
        }
    }
}
