namespace Ukrainians.UtilityServices.Services.ChatRoom
{
    public interface IChatRoomBaseService<T>
    {
        Task<IEnumerable<T>> GetAllChatRooms();
        Task<IEnumerable<string>> GetUsernamesUserInteractedWith(string username);
        Task<T?> GetChatRoomById(Guid id);
        Task<T?> GetChatRoomByName(string name);
        Task<T> AddChatRoom(T room);
        Task<T> UpdateChatRoom(T room);
        Task<bool> DeleteChatRoom(Guid id);
        Task<IEnumerable<T>> GetChatRoomsUserInteractedWith(string username);
    }
}
