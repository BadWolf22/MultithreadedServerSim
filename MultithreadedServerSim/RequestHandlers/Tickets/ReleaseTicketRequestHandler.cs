using MultithreadedServerSim.Attributes;
using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Exceptions;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.RequestHandlers.Tickets;

[Controller("tickets")]
[Route("{int:ticketId}", "r")]
[Route("{int:ticketId}", "release")]
[Route("{int:ticketId}", "refund")]
[Route("{int:ticketId}", "cancel")]
internal class ReleaseTicketRequestHandler(ITicketService ticketService) : IRequestHandler
{
    public Response HandleRequest(dynamic details)
    {
        int ticketId = details.ticketId;
        try
        {
            ticketService.ReleaseTicket(ticketId);
        }
        catch (NotFoundException)
        {
            return new Response(404, $"The ticket with ID {ticketId} was not found.");
        }

        return new Response(200, $"Ticket {ticketId} has been released");
    }
}
