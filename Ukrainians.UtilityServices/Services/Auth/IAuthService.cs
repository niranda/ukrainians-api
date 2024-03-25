using Ukrainians.UtilityServices.Models.Auth;

namespace Ukrainians.UtilityServices.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResultModel> Login(AuthModel authModel);
        Task<AuthResultModel> Signup(AuthModel authModel);
        Task<bool> ConfirmEmail(string email, string token);
    }
}
