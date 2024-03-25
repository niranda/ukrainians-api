using Ukrainians.Domain.Core.Models;
using Ukrainians.UtilityServices.Services.ChatMessage;

namespace NomadChat.WebAPI.Services
{
    public class ChatMessageService : IChatMessageService<ChatMessageDomain>
    {
        private readonly IChatMessageBaseService<ChatMessageDomain> _chatMessageDomain;
        private readonly ILogger _logger;

        public ChatMessageService(IChatMessageBaseService<ChatMessageDomain> chatMessageDomain, ILoggerFactory loggerFactory)
        {
            _chatMessageDomain = chatMessageDomain;
            _logger = loggerFactory.CreateLogger<ChatMessageService>();
        }

        public async Task<IEnumerable<ChatMessageDomain>> GetAllChatMessages()
        {
            _logger.LogInformation($"Request to get all chat messages");
            return await _chatMessageDomain.GetAllChatMessages();
        }

        public async Task<IEnumerable<ChatMessageDomain>> GetAllChatMessagesByRoomId(Guid roomId)
        {
            _logger.LogInformation($"Request to get all chat messages by room id {roomId}");
            return await _chatMessageDomain.GetAllChatMessagesByRoomId(roomId);
        }

        public async Task<ChatMessageDomain> GetChatMessageById(Guid id)
        {
            _logger.LogInformation($"Request to get chat message by id {id}");
            return await _chatMessageDomain.GetChatMessageById(id);
        }

        public async Task<ChatMessageDomain> AddChatMessage(ChatMessageDomain message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _logger.LogInformation($"Request to add a chat message");
            var res = await _chatMessageDomain.AddChatMessage(message);
            return res;
        }

        public async Task<ChatMessageDomain> UpdateChatMessage(ChatMessageDomain message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _logger.LogInformation($"Request to update chat message");
            return await _chatMessageDomain.UpdateChatMessage(message);
        }

        public async Task<IEnumerable<ChatMessageDomain>> UpdateChatMessages(IEnumerable<ChatMessageDomain> messages)
        {
            _logger.LogInformation($"Request to update chat messages");
            return await _chatMessageDomain.UpdateChatMessages(messages);
        }

        public async Task<bool> DeleteChatMessage(Guid id)
        {
            _logger.LogInformation($"Request to delete chat message");
            return await _chatMessageDomain.DeleteChatMessage(id);
        }

        public async Task<ChatMessageDomain?> GetLastMessageByChatRoomId(Guid chatRoomId)
        {
            _logger.LogInformation($"Request to get last message by chat room id {chatRoomId}");
            return await _chatMessageDomain.GetLastMessageByChatRoomId(chatRoomId);
        }

        public string DecryptMessage(string content)
        {
            _logger.LogInformation($"Request to decrypt a message");
            return _chatMessageDomain.DecryptMessage(content);
        }
    }
}
