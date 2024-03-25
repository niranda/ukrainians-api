using Ukrainians.UtilityServices.Models.Base;

namespace Ukrainians.UtilityServices.Models.Common
{
    public class PushNotificationsSubscriptionModel : BaseModel
    {
        public string Endpoint { get; set; }
        public string P256DH { get; set; }
        public string Auth { get; set; }
        public string? Username { get; set; }
    }
}
