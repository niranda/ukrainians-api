using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.UtilityServices.Models.Common;

namespace Ukrainians.Domain.Core.Models
{
    public class ChatRoomDomain : ChatRoomModel
    {
        public ICollection<ChatMessageDomain> ChatMessages { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<ChatNotificationDomain> Notifications { get; set; }
    }
}
