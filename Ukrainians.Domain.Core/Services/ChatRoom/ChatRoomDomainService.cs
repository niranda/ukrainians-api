using AutoMapper;
using Ukrainians.Domain.Core.Models;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;
using Ukrainians.UtilityServices.Services.ChatRoom;

namespace Ukrainians.Domain.Core.Services.ChatRoomD
{
    public class ChatRoomDomainService : IChatRoomBaseService<ChatRoomDomain>
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IMapper _mapper;

        public ChatRoomDomainService(
            IChatRoomRepository chatRoomRepository,
            IMapper mapper)
        {
            _chatRoomRepository = chatRoomRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChatRoomDomain>> GetAllChatRooms()
        {
            return _mapper.Map<IEnumerable<ChatRoomDomain>>(await _chatRoomRepository.GetAll());
        }

        public async Task<IEnumerable<string>> GetUsernamesUserInteractedWith(string username)
        {
            var rooms = await _chatRoomRepository.GetRoomsUserInteractedWith(username);

            return rooms.Select(x => x.RoomName!.Split('-').FirstOrDefault(s => s != username))!;
        }

        public async Task<IEnumerable<ChatRoomDomain>> GetChatRoomsUserInteractedWith(string username)
        {
            return _mapper.Map<IEnumerable<ChatRoomDomain>>(await _chatRoomRepository.GetRoomsUserInteractedWith(username));
        }

        public async Task<ChatRoomDomain> GetChatRoomById(Guid id)
        {
            var room = await _chatRoomRepository.GetById(id);

            return _mapper.Map<ChatRoomDomain>(room);
        }

        public async Task<ChatRoomDomain?> GetChatRoomByName(string name)
        {
            var room = await _chatRoomRepository.GetByName(name);

            return _mapper.Map<ChatRoomDomain>(room);
        }

        public async Task<ChatRoomDomain> AddChatRoom(ChatRoomDomain roomDomain)
        {
            var chatRoom = _mapper.Map<ChatRoom>(roomDomain);

            return _mapper.Map<ChatRoomDomain>(await _chatRoomRepository.Create(chatRoom));
        }

        public async Task<ChatRoomDomain> UpdateChatRoom(ChatRoomDomain roomDomain)
        {
            var room = _mapper.Map<ChatRoom>(roomDomain);

            return _mapper.Map<ChatRoomDomain>(await _chatRoomRepository.Update(room));
        }

        public async Task<bool> DeleteChatRoom(Guid id)
        {
            var roomToDelete = await _chatRoomRepository.GetById(id, false);

            if (roomToDelete == null)
            {
                throw new NullReferenceException(nameof(roomToDelete));
            }

            return await _chatRoomRepository.Delete(roomToDelete);
        }
    }
}
