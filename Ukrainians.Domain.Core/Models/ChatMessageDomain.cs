using Ukrainians.UtilityServices.Models.Common;

namespace Ukrainians.Domain.Core.Models
{
    public class ChatMessageDomain : ChatMessageModel
    {
        public Guid? ChatRoomId { get; set; }

        public ChatRoomDomain ChatRoom { get; set; }
    }
}
