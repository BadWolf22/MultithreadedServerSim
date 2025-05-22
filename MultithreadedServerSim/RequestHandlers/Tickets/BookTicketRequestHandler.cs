using MultithreadedServerSim.Attributes;
using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Exceptions;
using MultithreadedServerSim.Interfaces;

namespace MultithreadedServerSim.RequestHandlers.Tickets;

[Controller("tickets")]
[Route("{int:ticketId}", "b")]
[Route("{int:ticketId}", "book")]
[Route("{int:ticketId}", "buy")]
internal class BookTicketRequestHandler(ITicketService ticketService) : IRequestHandler
{
    public Response HandleRequest(dynamic details)
    {
        int ticketId = details.ticketId;
        try
        {
            ticketService.BookTicket(ticketId, details.body);
        }
        catch (NotFoundException)
        {
            return new Response(404, $"The ticket with ID {ticketId} was not found.");
        }

        return new Response(200, $"Ticket {ticketId} has been booked");
    }
}
