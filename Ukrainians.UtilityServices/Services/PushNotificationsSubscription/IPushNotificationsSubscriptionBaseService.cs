namespace Ukrainians.UtilityServices.Services.PushNotificationsSubscription
{
    public interface IPushNotificationsSubscriptionBaseService<T>
    {
        Task<IEnumerable<T>> GetAllPushNotificationsSubscriptions();
        Task<T> GetPushNotificationsSubscriptionById(Guid id);
        Task<T> GetPushNotificationsSubscriptionByUsername(string username);
        Task<T> AddPushNotificationsSubscription(T pushNotificationsSubscription);
        Task<bool> DeletePushNotificationsSubscription(Guid id);
    }
}
