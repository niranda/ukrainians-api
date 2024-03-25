using Ukrainians.Domain.Core.Models;
using Ukrainians.UtilityServices.Services.PushNotificationsSubscription;

namespace NomadChat.WebAPI.Services
{
    public class PushNotificationsSubscriptionService : IPushNotificationsSubscriptionService<PushNotificationsSubscriptionDomain>
    {
        private readonly IPushNotificationsSubscriptionBaseService<PushNotificationsSubscriptionDomain> _pushNotificationsSubscriptionDomainService;
        private readonly ILogger _logger;

        public PushNotificationsSubscriptionService(IPushNotificationsSubscriptionBaseService<PushNotificationsSubscriptionDomain> pushNotificationsSubscriptionBaseService,
            ILoggerFactory loggerFactory)
        {
            _pushNotificationsSubscriptionDomainService = pushNotificationsSubscriptionBaseService;
            _logger = loggerFactory.CreateLogger<PushNotificationsSubscriptionService>();
        }

        public async Task<IEnumerable<PushNotificationsSubscriptionDomain>> GetAllPushNotificationsSubscriptions()
        {
            _logger.LogInformation($"Request to get all push subscriptions");
            return await _pushNotificationsSubscriptionDomainService.GetAllPushNotificationsSubscriptions();
        }

        public async Task<PushNotificationsSubscriptionDomain> GetPushNotificationsSubscriptionByUsername(string username)
        {
            _logger.LogInformation($"Request to get push subscription by username {username}");
            return await _pushNotificationsSubscriptionDomainService.GetPushNotificationsSubscriptionByUsername(username);
        }

        public async Task<PushNotificationsSubscriptionDomain> GetPushNotificationsSubscriptionById(Guid id)
        {
            _logger.LogInformation($"Request to get push subscription by id {id}");
            return await _pushNotificationsSubscriptionDomainService.GetPushNotificationsSubscriptionById(id);
        }

        public Task<PushNotificationsSubscriptionDomain> AddPushNotificationsSubscription(PushNotificationsSubscriptionDomain pushNotificationsSubscription)
        {
            if (pushNotificationsSubscription == null)
            {
                throw new ArgumentNullException(nameof(pushNotificationsSubscription));
            }

            _logger.LogInformation($"Request to add push subscription");
            return _pushNotificationsSubscriptionDomainService.AddPushNotificationsSubscription(pushNotificationsSubscription);
        }

        public async Task<bool> DeletePushNotificationsSubscription(Guid id)
        {
            _logger.LogInformation($"Request to delete push subscription");
            return await _pushNotificationsSubscriptionDomainService.DeletePushNotificationsSubscription(id);
        }
    }
}
