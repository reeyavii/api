using API.Models;

namespace API.Services
{
    public interface INotificationService
    {
        Notification SendNotif(string userId, string message, string title);
    }
}
