using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Exceptions;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.Repos;

internal class InMemoryTicketRepository : ITicketRepository
{
    private readonly Dictionary<int, Ticket> _availableTickets = [];
    private readonly Dictionary<int, Ticket> _bookedTickets = [];

    public InMemoryTicketRepository()
    {
        for (var i = 0; i < 3; i++)
        {
            _availableTickets.Add(i, new Ticket(i, null));
        }
    }

    public HashSet<int> GetAvailableTicketIds()
    {
        return [.. _availableTickets.Keys];
    }

    public void BookTicket(int id, string userName)
    {
        Thread.Sleep(1000);
        if (!_availableTickets.Remove(id, out var ticket))
            throw new NotFoundException();
        _bookedTickets.Add(id, ticket with { UserName = userName });
    }

    public void ReleaseTicket(int id)
    {
        Thread.Sleep(1000);
        if (!_bookedTickets.Remove(id, out var ticket))
            throw new NotFoundException();
        _availableTickets.Add(id, ticket with { UserName = null });
    }
}