using AutoMapper;
using Ukrainians.Domain.Core.Models;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;
using Ukrainians.UtilityServices.Services.PushNotificationsSubscription;

namespace Ukrainians.Domain.Core.Services.PushNotificationsSubscriptionD
{
    public class PushNotificationsSubscriptionDomainService : IPushNotificationsSubscriptionBaseService<PushNotificationsSubscriptionDomain>
    {
        private readonly IPushNotificationsSubscriptionRepository _pushNotificationsSubscriptionRepository;
        private readonly IMapper _mapper;

        public PushNotificationsSubscriptionDomainService(IPushNotificationsSubscriptionRepository pushNotificationsSubscriptionRepository,
            IMapper mapper)
        {
            _pushNotificationsSubscriptionRepository = pushNotificationsSubscriptionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PushNotificationsSubscriptionDomain>> GetAllPushNotificationsSubscriptions()
        {
            return _mapper.Map<IEnumerable<PushNotificationsSubscriptionDomain>>(await _pushNotificationsSubscriptionRepository.GetAll());
        }

        public async Task<PushNotificationsSubscriptionDomain> GetPushNotificationsSubscriptionByUsername(string username)
        {
            return _mapper.Map<PushNotificationsSubscriptionDomain>(await _pushNotificationsSubscriptionRepository.GetByUsername(username));
        }

        public async Task<PushNotificationsSubscriptionDomain> GetPushNotificationsSubscriptionById(Guid id)
        {
            return _mapper.Map<PushNotificationsSubscriptionDomain>(await _pushNotificationsSubscriptionRepository.GetById(id));
        }

        public async Task<PushNotificationsSubscriptionDomain> AddPushNotificationsSubscription(PushNotificationsSubscriptionDomain pushNotificationsSubscription)
        {
            var pushSubscription = _mapper.Map<PushNotificationsSubscription>(pushNotificationsSubscription);
            return _mapper.Map<PushNotificationsSubscriptionDomain>(await _pushNotificationsSubscriptionRepository.Create(pushSubscription));
        }

        public async Task<bool> DeletePushNotificationsSubscription(Guid id)
        {
            var subscriptionToDelete = await _pushNotificationsSubscriptionRepository.GetById(id, false);

            if (subscriptionToDelete == null)
            {
                throw new NullReferenceException(nameof(subscriptionToDelete));
            }

            return await _pushNotificationsSubscriptionRepository.Delete(subscriptionToDelete);
        }
    }
}
