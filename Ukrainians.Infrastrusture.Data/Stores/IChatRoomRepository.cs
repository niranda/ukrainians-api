using Ukrainians.Infrastrusture.Data.Entities;

namespace Ukrainians.Infrastrusture.Data.Stores
{
    public interface IChatRoomRepository
    {
        Task<IEnumerable<ChatRoom>> GetAll();
        Task<IEnumerable<ChatRoom>> GetRoomsUserInteractedWith(string username);
        Task<ChatRoom?> GetById(Guid id, bool asNoTracking = true);
        Task<ChatRoom?> GetByName(string name, bool asNoTracking = true);
        Task<ChatRoom> Create(ChatRoom room);
        Task<ChatRoom> Update(ChatRoom room);
        Task<bool> Delete(ChatRoom room);
    }
}
