using Microsoft.Extensions.DependencyInjection;
using Ukrainians.Infrastrusture.Data.Repositories;
using Ukrainians.Infrastrusture.Data.Stores;

namespace Ukrainians.Infrastrusture.Data.Configs
{
    public static class DIConfig
    {
        public static void RegisterInfrastructureDataInjections(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IChatNotificationRepository, ChatNotificationRepository>();
            services.AddScoped<IPushNotificationsSubscriptionRepository, PushNotificationsSubscriptionRepository>();
        }
    }
}
