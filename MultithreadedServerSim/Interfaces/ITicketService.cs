namespace MultithreadedServerSim.Interfaces;

internal interface ITicketService
{
    HashSet<int> GetAvailableTicketIds();
    void BookTicket(int id, string userName);
    void ReleaseTicket(int id);
}
