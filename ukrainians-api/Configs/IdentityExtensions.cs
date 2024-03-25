using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ukrainians.Infrastrusture.Data.Context;
using Ukrainians.Infrastrusture.Data.Entities;

namespace NomadChat.WebAPI.Configs
{
    public static class IdentityExtensions
    {
        public static void SetupIdentity(this IServiceCollection serviceCollection, bool enableDefaultTokenProviders = true)
        {
            serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.TryAddScoped<UserManager<User>>();
            serviceCollection.TryAddScoped<RoleManager<Role>>();

            var identityBuilder = serviceCollection.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
            });

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(Role), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<ApplicationContext>();

            if (enableDefaultTokenProviders)
            {
                identityBuilder.AddDefaultTokenProviders();
            }
        }
    }
}
