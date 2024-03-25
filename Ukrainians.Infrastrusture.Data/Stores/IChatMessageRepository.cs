using Ukrainians.Infrastrusture.Data.Entities;

namespace Ukrainians.Infrastrusture.Data.Stores
{
    public interface IChatMessageRepository
    {
        Task<IEnumerable<ChatMessage>> GetAll();
        Task<ChatMessage?> GetById(Guid id, bool asNoTracking = true);
        Task<IEnumerable<ChatMessage>> GetAllByRoomId(Guid roomId);
        Task<ChatMessage> Create(ChatMessage message);
        Task<ChatMessage> Update(ChatMessage message);
        Task<IEnumerable<ChatMessage>> UpdateRange(IEnumerable<ChatMessage> message);
        Task<bool> Delete(ChatMessage message);
    }
}
