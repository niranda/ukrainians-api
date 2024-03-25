using Ukrainians.UtilityServices.Models.Common;

namespace Ukrainians.UtilityServices.Services.Chat
{
    public interface IChatService
    {
        bool AddUserToList(UserModel user);
        void AddUserConnectionId(string user, string connectionId);
        void AddUserNotificationId(string user, string notificationId);
        string GetNotificationIdByUser(string user);
        string GetUserByConnectionId(string connectionId);
        string GetConnectionIdByUser(string user);
        void RemoveUserFromList(string user);
        void RemoveUserNotificationId(string user);
        string[] GetOnlineUsers();
    }
}
