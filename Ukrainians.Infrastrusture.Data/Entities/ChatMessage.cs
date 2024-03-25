using System.ComponentModel.DataAnnotations;
using Ukrainians.Infrastrusture.Data.Entities.Base;

namespace Ukrainians.Infrastrusture.Data.Entities
{
    public class ChatMessage : BaseEntity
    {
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        [StringLength(50)]
        public string From { get; set; }
        public string? To { get; set; }
        public bool Unread { get; set; }

        public Guid? ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
