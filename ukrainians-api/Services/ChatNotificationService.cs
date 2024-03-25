using Ukrainians.Domain.Core.Models;
using Ukrainians.UtilityServices.Services.ChatNotification;

namespace NomadChat.WebAPI.Services
{
    public class ChatNotificationService : IChatNotificationService<ChatNotificationDomain>
    {
        private readonly IChatNotificationBaseService<ChatNotificationDomain> _chatNotificationService;
        private readonly ILogger _logger;

        public ChatNotificationService(IChatNotificationBaseService<ChatNotificationDomain> chatNotificationService, ILoggerFactory loggerFactory)
        {
            _chatNotificationService = chatNotificationService;
            _logger = loggerFactory.CreateLogger<ChatNotificationService>();
        }

        public async Task<IEnumerable<ChatNotificationDomain>> GetChatNotificationsByUsername(string username)
        {
            _logger.LogInformation($"Request to get chat notifications by username: {username}");
            return await _chatNotificationService.GetChatNotificationsByUsername(username);
        }

        public async Task<ChatNotificationDomain?> GetChatNotificationByUsernameAndRoomId(string username, Guid chatRoomId)
        {
            _logger.LogInformation($"Request to get chat notifications by username: {username} and chatRoomId");
            return await _chatNotificationService.GetChatNotificationByUsernameAndRoomId(username, chatRoomId);
        }

        public async Task<ChatNotificationDomain> AddChatNotification(ChatNotificationDomain chatNotification)
        {
            _logger.LogInformation($"Request to add chat notification");
            return await _chatNotificationService.AddChatNotification(chatNotification);
        }

        public async Task<ChatNotificationDomain> UpdateChatNotification(ChatNotificationDomain chatNotification)
        {
            _logger.LogInformation($"Request to update chat notification");
            return await _chatNotificationService.UpdateChatNotification(chatNotification);
        }
    }
}
