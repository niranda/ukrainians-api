using System.ComponentModel.DataAnnotations;
using Ukrainians.Infrastrusture.Data.Entities.Base;

namespace Ukrainians.Infrastrusture.Data.Entities
{
    public class ChatRoom : BaseEntity
    {
        [StringLength(50)]
        public string? RoomName { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<ChatNotification> Notifications { get; set; }
    }
}
