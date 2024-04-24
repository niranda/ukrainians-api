using Ukrainians.Infrastructure.Business.Services.Auth;
using Ukrainians.UtilityServices.Models.Auth;
using Ukrainians.UtilityServices.Services.Auth;

namespace NomadChat.WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthInfrastructureService _authService;
        private readonly ILogger _logger;
        public AuthService(IAuthInfrastructureService authService, ILoggerFactory loggerFactory)
        {
            _authService = authService;
            _logger = loggerFactory.CreateLogger<AuthService>();
        }

        public async Task<bool> ConfirmEmail(string email, string token)
        {
            _logger.LogInformation($"Request to confirm an email {email}");
            return await _authService.ConfirmEmail(email, token);
        }

        public Task<AuthResultModel> Login(AuthModel authModel)
        {
            if (authModel == null)
            {
                throw new ArgumentNullException(nameof(authModel));
            }

            _logger.LogInformation($"Request to log in a user with email {authModel.UserName}");
            return _authService.Login(authModel);
        }

        public Task<AuthResultModel> Signup(AuthModel authModel)
        {
            if (authModel == null)
            {
                throw new ArgumentNullException(nameof(authModel));
            }

            _logger.LogInformation($"Request to sign-up a user");
            return _authService.Signup(authModel);
        }
    }
}
