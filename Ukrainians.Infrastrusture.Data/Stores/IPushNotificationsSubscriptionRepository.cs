using Ukrainians.Infrastrusture.Data.Entities;

namespace Ukrainians.Infrastrusture.Data.Stores
{
    public interface IPushNotificationsSubscriptionRepository
    {
        Task<IEnumerable<PushNotificationsSubscription>> GetAll();
        Task<PushNotificationsSubscription?> GetById(Guid id, bool asNoTracking = true);
        Task<PushNotificationsSubscription?> GetByUsername(string username);
        Task<PushNotificationsSubscription> Create(PushNotificationsSubscription pushNotificationsSubscription);
        Task<bool> Delete(PushNotificationsSubscription pushNotificationsSubscription);
    }
}
