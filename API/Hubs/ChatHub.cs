using Api.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    public class ChatHub : Hub<IChatClient>
    { }
}
