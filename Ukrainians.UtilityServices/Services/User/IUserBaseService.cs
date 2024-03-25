namespace Ukrainians.UtilityServices.Services.User
{
    public interface IUserBaseService<T>
    {
        Task<Guid> CreateUser(string role, string email, string username);
        Task<T?> GetUserByEmail(string email);
        Task<IEnumerable<T>> GetAllUsers();
        Task<T> UpdateUser(T user);
        Task<bool> DeleteUser(T user);
        //Task<byte[]> SetProfilePicture(Guid id, IFormFile file);
    }
}
