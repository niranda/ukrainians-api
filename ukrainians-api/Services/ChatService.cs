using Ukrainians.UtilityServices.Models.Common;
using Ukrainians.UtilityServices.Services.Chat;
using WebPush;

namespace NomadChat.WebAPI.Services
{
    public class ChatService : IChatService
    {
        private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> UsersNotifications = new Dictionary<string, string>();

        private readonly ILogger _logger;
        public ChatService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ChatService>();
        }

        public bool AddUserToList(UserModel userToAdd)
        {
            lock (Users)
            {
                foreach (var user in Users)
                {
                    if (user.Key.ToLower() == userToAdd.Name.ToLower() || user.Key.ToLower() == userToAdd.Email?.ToLower()) 
                    {
                        RemoveUserFromList(user.Key);
                        RemoveUserNotificationId(user.Key);
                    };
                }

                Users.Add(userToAdd.Name, string.Empty);
                UsersNotifications.Add(userToAdd.Name, string.Empty);

                _logger.LogInformation($"User {userToAdd} is online");
                return true;
            }
        }

        public void AddUserConnectionId(string user, string connectionId)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users[user] = connectionId;
                    _logger.LogInformation($"User {user} connection has been added successfully");
                }
            }
        }

        public void AddUserNotificationId(string user, string notificationId)
        {
            lock (UsersNotifications)
            {
                if (UsersNotifications.ContainsKey(user))
                {
                    UsersNotifications[user] = notificationId;
                    _logger.LogInformation($"User {user} notificationId has been added successfully");
                }
            }
        }

        public void RemoveUserNotificationId(string user)
        {
            lock (UsersNotifications)
            {
                if (UsersNotifications.ContainsKey(user))
                {
                    UsersNotifications[user] = string.Empty;
                    _logger.LogInformation($"User {user} notificationId has been removed successfully");
                }
            }
        }

        public string GetNotificationIdByUser(string user)
        {
            lock (UsersNotifications)
            {
                return UsersNotifications.FirstOrDefault(x => x.Key == user).Value;
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock (Users)
            {
                return Users.FirstOrDefault(x => x.Value == connectionId).Key;
            }
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (Users)
            {
                return Users.FirstOrDefault(x => x.Key == user).Value;
            }
        }

        public void RemoveUserFromList(string user)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users.Remove(user);
                    UsersNotifications.Remove(user);
                    _logger.LogInformation($"User {user} is offline");
                }
            }
        }

        public string[] GetOnlineUsers()
        {
            lock (Users)
            {
                return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }
    }
}
