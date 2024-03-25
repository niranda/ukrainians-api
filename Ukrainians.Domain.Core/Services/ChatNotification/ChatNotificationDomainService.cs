using AutoMapper;
using Ukrainians.Domain.Core.Models;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;
using Ukrainians.UtilityServices.Services.ChatNotification;

namespace Ukrainians.Domain.Core.Services.ChatNotificationD
{
    internal class ChatNotificationDomainService : IChatNotificationBaseService<ChatNotificationDomain>
    {
        private readonly IChatNotificationRepository _chatNotificationRepository;
        private readonly IMapper _mapper;

        public ChatNotificationDomainService(IChatNotificationRepository chatNotificationRepository, IMapper mapper)
        {
            _chatNotificationRepository = chatNotificationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatNotificationDomain>> GetChatNotificationsByUsername(string username)
        {
            return _mapper.Map<IEnumerable<ChatNotificationDomain>>(await _chatNotificationRepository.GetAllByUsername(username));
        }

        public async Task<ChatNotificationDomain?> GetChatNotificationByUsernameAndRoomId(string username, Guid chatRoomId)
        {
            return _mapper.Map<ChatNotificationDomain>(await _chatNotificationRepository.GetByUsernameAndRoomId(username, chatRoomId));
        }

        public async Task<ChatNotificationDomain> AddChatNotification(ChatNotificationDomain notificationDomain)
        {
            var chatNotification = _mapper.Map<ChatNotification>(notificationDomain);

            return _mapper.Map<ChatNotificationDomain>(await _chatNotificationRepository.Create(chatNotification));
        }

        public async Task<ChatNotificationDomain> UpdateChatNotification(ChatNotificationDomain notificationDomain)
        {
            var chatNotification = _mapper.Map<ChatNotification>(notificationDomain);

            return _mapper.Map<ChatNotificationDomain>(await _chatNotificationRepository.Update(chatNotification));
        }
    }
}
