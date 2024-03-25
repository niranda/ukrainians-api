namespace Ukrainians.UtilityServices.Services.ChatNotification
{
    public interface IChatNotificationBaseService<T>
    {
        Task<T> AddChatNotification(T chatNotification);
        Task<T> UpdateChatNotification(T chatNotification);
        Task<IEnumerable<T>> GetChatNotificationsByUsername(string username);
        Task<T?> GetChatNotificationByUsernameAndRoomId(string username, Guid chatRoomId);
    }
}
