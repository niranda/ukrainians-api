namespace Ukrainians.UtilityServices.Services.ChatMessage
{
    public interface IChatMessageBaseService<T>
    {
        Task<IEnumerable<T>> GetAllChatMessages();
        Task<IEnumerable<T>> GetAllChatMessagesByRoomId(Guid roomId);
        Task<T> GetChatMessageById(Guid id);
        Task<T> AddChatMessage(T message);
        Task<T> UpdateChatMessage(T message);
        Task<IEnumerable<T>> UpdateChatMessages(IEnumerable<T> messages);
        Task<bool> DeleteChatMessage(Guid id);
        Task<T?> GetLastMessageByChatRoomId(Guid chatRoomId);
        string DecryptMessage(string content);
    }
}
