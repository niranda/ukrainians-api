using AutoMapper;
using Ukrainians.Domain.Core.Models;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.UtilityServices.Services.Encryption;
using Ukrainians.UtilityServices.Settings;

namespace NomadChat.WebAPI.Settings
{
    public class MapperProfile : Profile
    {
        private readonly EncryptionSettings _settings;

        public MapperProfile(EncryptionSettings settings) 
        {
            _settings = settings;

            CreateMap<ChatMessage, ChatMessageDomain>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => EncryptionService.DecryptPicture(src.Picture, _settings.PictureKey)))
                .ReverseMap()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => EncryptionService.EncryptPicture(src.Picture, _settings.PictureKey)));

            CreateMap<ChatRoom, ChatRoomDomain>().ReverseMap();
            CreateMap<ChatNotification, ChatNotificationDomain>().ReverseMap();
            CreateMap<PushNotificationsSubscription, PushNotificationsSubscriptionDomain>().ReverseMap();
        }
    }
}
