using Ukrainians.Domain.Core.Models;
using Ukrainians.UtilityServices.Services.ChatRoom;

namespace NomadChat.WebAPI.Services
{
    public class ChatRoomService : IChatRoomService<ChatRoomDomain>
    {
        private readonly IChatRoomBaseService<ChatRoomDomain> _chatRoomDomain;
        private readonly ILogger _logger;

        public ChatRoomService(IChatRoomBaseService<ChatRoomDomain> chatRoomDomain, ILoggerFactory loggerFactory)
        {
            _chatRoomDomain = chatRoomDomain;
            _logger = loggerFactory.CreateLogger<ChatRoomService>();
        }

        public async Task<IEnumerable<ChatRoomDomain>> GetAllChatRooms()
        {
            _logger.LogInformation($"Request to get all chat rooms");
            return await _chatRoomDomain.GetAllChatRooms();
        }

        public async Task<IEnumerable<string>> GetUsernamesUserInteractedWith(string username)
        {
            _logger.LogInformation($"Request to get all username {username} interacted with");
            return await _chatRoomDomain.GetUsernamesUserInteractedWith(username);
        }

        public async Task<ChatRoomDomain?> GetChatRoomById(Guid id)
        {
            _logger.LogInformation($"Request to get chat room by id {id}");
            return await _chatRoomDomain.GetChatRoomById(id);
        }

        public async Task<ChatRoomDomain?> GetChatRoomByName(string name)
        {
            _logger.LogInformation($"Request to get chat room by name {name}");
            return await _chatRoomDomain.GetChatRoomByName(name);
        }

        public async Task<ChatRoomDomain> AddChatRoom(ChatRoomDomain room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            _logger.LogInformation($"Request to add a new chat room");
            return await _chatRoomDomain.AddChatRoom(room);
        }

        public async Task<ChatRoomDomain> UpdateChatRoom(ChatRoomDomain room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room));
            }

            _logger.LogInformation($"Request to update chat room");
            return await _chatRoomDomain.UpdateChatRoom(room);
        }

        public async Task<bool> DeleteChatRoom(Guid id)
        {
            _logger.LogInformation($"Request to delete chat room");
            return await _chatRoomDomain.DeleteChatRoom(id);
        }

        public async Task<IEnumerable<ChatRoomDomain>> GetChatRoomsUserInteractedWith(string username)
        {
            _logger.LogInformation($"Request to get chat rooms {username} interacted with");
            return await _chatRoomDomain.GetChatRoomsUserInteractedWith(username);
        }
    }
}
