using MultithreadedServerSim.Attributes;
using MultithreadedServerSim.Contracts;
using MultithreadedServerSim.Interfaces;
using System.Text.Json;

namespace MultithreadedServerSim.RequestHandlers.Tickets;

[Controller("tickets")]
[Route("g")]
[Route("get")]
internal class GetTicketsRequestHandler(ITicketService ticketService) : IRequestHandler
{
    public Response HandleRequest(dynamic details)
    {
        var ticketIds = ticketService.GetAvailableTicketIds();
        return new Response(200, JsonSerializer.Serialize(ticketIds));
    }
}
