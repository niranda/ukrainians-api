using Ukrainians.UtilityServices.Models.Common;

namespace Ukrainians.Domain.Core.Models
{
    public class ChatNotificationDomain : ChatNotificationModel
    {
        public Guid? ChatRoomId { get; set; }
        public ChatRoomDomain ChatRoom { get; set; }
    }
}
