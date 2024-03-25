using AutoMapper;
using Ukrainians.Domain.Core.Models;
using Ukrainians.Infrastrusture.Data.Entities;

namespace NomadChat.WebAPI.Settings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ChatMessage, ChatMessageDomain>().ReverseMap();
            CreateMap<ChatRoom, ChatRoomDomain>().ReverseMap();
            CreateMap<ChatNotification, ChatNotificationDomain>().ReverseMap();
            CreateMap<PushNotificationsSubscription, PushNotificationsSubscriptionDomain>().ReverseMap();
        }
    }
}
