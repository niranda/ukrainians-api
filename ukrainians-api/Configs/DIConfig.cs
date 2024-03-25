using NomadChat.WebAPI.Services;
using Ukrainians.Domain.Core.Models;
using Ukrainians.UtilityServices.Services.Auth;
using Ukrainians.UtilityServices.Services.Chat;
using Ukrainians.UtilityServices.Services.ChatMessage;
using Ukrainians.UtilityServices.Services.ChatNotification;
using Ukrainians.UtilityServices.Services.ChatRoom;
using Ukrainians.UtilityServices.Services.PushNotificationsSubscription;

namespace NomadChat.WebAPI.Configs
{
    public static class DIConfig
    {
        public static void RegisterWebAPIInjections(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IChatMessageService<ChatMessageDomain>, ChatMessageService>();
            services.AddScoped<IChatRoomService<ChatRoomDomain>, ChatRoomService>();
            services.AddSingleton<IChatService, ChatService>();
            services.AddScoped<IChatNotificationService<ChatNotificationDomain>, ChatNotificationService>();
            services.AddScoped<IPushNotificationsSubscriptionService<PushNotificationsSubscriptionDomain>, PushNotificationsSubscriptionService>();
        }
    }
}
