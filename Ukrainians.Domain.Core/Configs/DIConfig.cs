using Microsoft.Extensions.DependencyInjection;
using Ukrainians.Domain.Core.Models;
using Ukrainians.Domain.Core.Services.ChatMessageD;
using Ukrainians.Domain.Core.Services.ChatNotificationD;
using Ukrainians.Domain.Core.Services.ChatRoomD;
using Ukrainians.Domain.Core.Services.PushNotificationsSubscriptionD;
using Ukrainians.Domain.Core.Services.UserD;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.UtilityServices.Services.ChatMessage;
using Ukrainians.UtilityServices.Services.ChatNotification;
using Ukrainians.UtilityServices.Services.ChatRoom;
using Ukrainians.UtilityServices.Services.PushNotificationsSubscription;
using Ukrainians.UtilityServices.Services.User;

namespace Ukrainians.Domain.Core.Configs
{
    public static class DIConfig
    {
        public static void RegisterDomainInjections(this IServiceCollection services)
        {
            services.AddScoped<IChatRoomBaseService<ChatRoomDomain>, ChatRoomDomainService>();
            services.AddScoped<IChatMessageBaseService<ChatMessageDomain>, ChatMessageDomainService>();
            services.AddScoped<IUserBaseService<User>, UserDomainService>();
            services.AddScoped<IChatNotificationBaseService<ChatNotificationDomain>, ChatNotificationDomainService>();
            services.AddScoped<IPushNotificationsSubscriptionBaseService<PushNotificationsSubscriptionDomain>, PushNotificationsSubscriptionDomainService>();
        }
    }
}
