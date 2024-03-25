using Ukrainians.UtilityServices.Models.Base;

namespace Ukrainians.UtilityServices.Models.Common
{
    public class ChatNotificationModel : BaseModel
    {
        public string Username { get; set; }
        public int UnreadMessages { get; set; }
    }
}
