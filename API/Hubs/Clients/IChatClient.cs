using System.Threading.Tasks;
using API.Models;

namespace Api.Hubs.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(Message message);
    }
}