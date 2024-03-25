using Ukrainians.UtilityServices.Models.Auth;

namespace Ukrainians.Infrastructure.Business.Services.Auth
{
    public interface IAuthInfrastructureService
    {
        Task<AuthResultModel> Login(AuthModel authModel);
        Task<AuthResultModel> Signup(AuthModel authModel);
        Task<bool> ConfirmEmail(string email, string token);
    }
}
