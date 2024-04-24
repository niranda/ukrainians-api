using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Ukrainians.Infrastructure.Business.Services.Email;
using Ukrainians.Infrastructure.Business.Services.Token;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;
using Ukrainians.UtilityServices.Common.Enums;
using Ukrainians.UtilityServices.Models.Auth;
using Ukrainians.UtilityServices.Models.Common;
using Ukrainians.UtilityServices.Services.Encryption;
using Ukrainians.UtilityServices.Settings;

namespace Ukrainians.Infrastructure.Business.Services.Auth
{
    public class AuthInfrastructureService : IAuthInfrastructureService
    {
        private readonly IEmailSenderService _emailSender;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly EncryptionSettings _settings;
        private readonly ILogger _logger;
        private static RoleManager<Role> _roleManager;
        private static UserManager<User> _userManager;

        public AuthInfrastructureService(
            IEmailSenderService emailSender,
            ITokenService tokenService,
            IUserRepository userRepository,
            EncryptionSettings encryptionSettings,
            ILoggerFactory loggerFactory,
            RoleManager<Role> roleManager,
            UserManager<User> userManager)
        {
            _emailSender = emailSender;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _settings = encryptionSettings;
            _logger = loggerFactory.CreateLogger<AuthInfrastructureService>();
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<AuthResultModel> Login(AuthModel authModel)
        {
            var user = await _userRepository.FindByUsername(authModel.UserName);

            if (user == null)
            {
                user = await _userRepository.FindByEmail(authModel.UserName);
            }

            if (user == null || !CheckPassword(user, authModel.Password))
            {
                _logger.LogInformation($"Unsuccessful login attempt with email {authModel.UserName}");
                return UnsuccessfulLogin(ErrorCode.InvalidLogin);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogInformation($"Unsuccessful login attempt with email {authModel.UserName}");
                return UnsuccessfulLogin(ErrorCode.InvalidEmail);
            }

            _logger.LogInformation($"User {authModel.UserName} successfully logged in");
            return new AuthResultModel
            {
                IsSuccess = true,
                Token = _tokenService.GenerateJWT(user)
            };
        }

        public async Task<AuthResultModel> Signup(AuthModel authModel)
        {
            var username = authModel.UserName;
            var email = authModel.Email;
            var isUser = await _userManager.FindByEmailAsync(email);

            if (isUser == null)
            {
                isUser = await _userManager.FindByNameAsync(username);
            }

            var role = authModel.Role;
            var testPassword = EncryptionService.Encrypt(authModel.Password, _settings.Key);

            var userRole = await _roleManager.FindByNameAsync(role);
            if (userRole == null)
            {
                await _roleManager.CreateAsync(new Role(role));
            }

            if (isUser == null)
            {
                userRole = await _roleManager.FindByNameAsync(role);

                if (string.IsNullOrEmpty(username))
                {
                    var random = new Random();
                    username = $"user-{random.Next(0, 1000000):D6}";
                }

                var user = new User(username, username, email, userRole!, false);

                await _userManager.CreateAsync(user);

                user.PasswordHash = testPassword;

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var parameters = new Dictionary<string, string?>
                {
                    { "token", token },
                    { "email", user.Email },
                };

                var callback = $"<h2>Please click the link below to confirm your email:</h2><p>{QueryHelpers.AddQueryString(authModel.ClientURI, parameters)}</p>";
                var message = new MessageModel(new string[] { user.Email }, "Email сonfirmation token", callback, null);
                await _emailSender.SendEmailAsync(message);

                await _userManager.AddToRoleAsync(user, role);
            }

            return await Login(authModel);
        }

        public async Task<bool> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return false;

            return true;
        }

        private bool CheckPassword(User user, string password)
        {
            return EncryptionService.Decrypt(user.PasswordHash, _settings.Key) == password;
        }

        private static AuthResultModel UnsuccessfulLogin(ErrorCode errorCode)
        {
            return new AuthResultModel { IsSuccess = false, ErrorMessage = errorCode };
        }
    }
}
