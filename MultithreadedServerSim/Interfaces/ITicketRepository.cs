namespace MultithreadedServerSim.Interfaces;

internal interface ITicketRepository
{
    HashSet<int> GetAvailableTicketIds();
    void BookTicket(int id, string userName);
    void ReleaseTicket(int id);
}
