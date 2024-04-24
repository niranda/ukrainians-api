using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Ukrainians.Domain.Core.Models;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;
using Ukrainians.UtilityServices.Services.ChatMessage;
using Ukrainians.UtilityServices.Services.ChatRoom;
using Ukrainians.UtilityServices.Services.Encryption;
using Ukrainians.UtilityServices.Settings;
using static System.Net.Mime.MediaTypeNames;

namespace Ukrainians.Domain.Core.Services.ChatMessageD
{
    public class ChatMessageDomainService : IChatMessageBaseService<ChatMessageDomain>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IChatRoomBaseService<ChatRoomDomain> _chatRoomDomainService;
        private readonly EncryptionSettings _settings;
        private readonly IMapper _mapper;

        public ChatMessageDomainService(
            IChatMessageRepository chatMessageRepository,
            EncryptionSettings settings,
            IMapper mapper,
            IChatRoomBaseService<ChatRoomDomain> chatRoomDomainService)
        {
            _chatMessageRepository = chatMessageRepository;
            _settings = settings;
            _mapper = mapper;
            _chatRoomDomainService = chatRoomDomainService;
        }
         
        public async Task<IEnumerable<ChatMessageDomain>> GetAllChatMessages()
        {
            var messages = (await _chatMessageRepository.GetAll()).ToList();
            messages.ForEach(s => s.Content = EncryptionService.Decrypt(s.Content, _settings.Key));
            return _mapper.Map<IEnumerable<ChatMessageDomain>>(messages);
        }

        public async Task<IEnumerable<ChatMessageDomain>> GetAllChatMessagesByRoomId(Guid roomId)
        {
            var messages = (await _chatMessageRepository.GetAllByRoomId(roomId)).ToList();
            messages.ForEach(s => s.Content = EncryptionService.Decrypt(s.Content, _settings.Key));

            var result = _mapper.Map<IEnumerable<ChatMessageDomain>>(messages);

            return result;
        }

        public async Task<ChatMessageDomain?> GetLastMessageByChatRoomId(Guid chatRoomId)
        {
            var chatRoom = await _chatRoomDomainService.GetChatRoomById(chatRoomId);
            if (chatRoom == null)
            {
                throw new NullReferenceException(nameof(chatRoom));
            }

            return chatRoom.ChatMessages?.FirstOrDefault();
        }

        public async Task<ChatMessageDomain> GetChatMessageById(Guid id)
        {
            var message = await _chatMessageRepository.GetById(id);

            if (message == null)
            {
                throw new NullReferenceException(nameof(message));
            }

            message.Content = EncryptionService.Decrypt(message.Content, _settings.Key);

            return _mapper.Map<ChatMessageDomain>(message);
        }

        public async Task<ChatMessageDomain> AddChatMessage(ChatMessageDomain messageDomain)
        {
            var chatMessage = _mapper.Map<ChatMessage>(messageDomain);

            var encryptedMessage = EncryptionService.Encrypt(messageDomain.Content, _settings.Key);
            chatMessage.Content = encryptedMessage;

            var result = await _chatMessageRepository.Create(chatMessage);

            result.Content = EncryptionService.Decrypt(result.Content, _settings.Key);

            return _mapper.Map<ChatMessageDomain>(result);
        }

        public async Task<ChatMessageDomain> UpdateChatMessage(ChatMessageDomain messageDomain)
        {
            var message = _mapper.Map<ChatMessage>(messageDomain);

            message.Content = EncryptionService.Encrypt(messageDomain.Content, _settings.Key);

            return _mapper.Map<ChatMessageDomain>(await _chatMessageRepository.Update(message));
        }

        public async Task<IEnumerable<ChatMessageDomain>> UpdateChatMessages(IEnumerable<ChatMessageDomain> messagesDomain)
        {
            var messagesToUpdate = messagesDomain.Select(m => new ChatMessageDomain
            {
                ChatRoomId = m.ChatRoomId,
                Content = EncryptionService.Encrypt(m.Content, _settings.Key),
                Created = m.Created,
                From = m.From,
                To = m.To,
                Id = m.Id,
                IsDeleted = m.IsDeleted,
                Unread = m.Unread,
                Picture = m.Picture,
            });

            var messages = _mapper.Map<IEnumerable<ChatMessage>>(messagesToUpdate);

            return _mapper.Map<IEnumerable<ChatMessageDomain>>(await _chatMessageRepository.UpdateRange(messages));
        }

        public async Task<bool> DeleteChatMessage(Guid id)
        {
            var messageToDelete = await _chatMessageRepository.GetById(id, false);

            if (messageToDelete == null)
            {
                throw new NullReferenceException(nameof(messageToDelete));
            }

            return await _chatMessageRepository.Delete(messageToDelete);
        }

        public string DecryptMessage(string content)
        {
            return EncryptionService.Decrypt(content, _settings.Key);
        }


    }
}
