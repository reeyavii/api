using API.Models;

namespace API.Services
{
    public class NotificationService : INotificationService
    {

        public Notification SendNotif(string userId, string message, string title)
        {
            var notifMessage = $"{userId} {message}";
            Notification notification = new Notification()
            {
                Message = notifMessage,
                UserId = userId,
                MessageStatus = Constants.messageStatusList[0],
                Title = title,
            };
            return notification;
           
        }
    }
}
