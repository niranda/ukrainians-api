using Ukrainians.Infrastrusture.Data.Entities;

namespace Ukrainians.Infrastrusture.Data.Stores
{
    public interface IChatNotificationRepository
    {
        Task<ChatNotification> Create(ChatNotification chatNotification);
        Task<ChatNotification> Update(ChatNotification chatNotification);
        Task<IEnumerable<ChatNotification>> GetAllByUsername(string username);
        Task<ChatNotification?> GetByUsernameAndRoomId(string username, Guid chatRoomId);
    }
}
