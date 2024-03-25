using Microsoft.Extensions.DependencyInjection;
using Ukrainians.Infrastructure.Business.Services.Auth;
using Ukrainians.Infrastructure.Business.Services.Email;
using Ukrainians.Infrastructure.Business.Services.Token;

namespace Ukrainians.Infrastructure.Business.Configs
{
    public static class DIConfig
    {
        public static void RegisterInfrastructureBusinessInjections(this IServiceCollection services)
        {
            services.AddScoped<IAuthInfrastructureService, AuthInfrastructureService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
        }
    }
}
