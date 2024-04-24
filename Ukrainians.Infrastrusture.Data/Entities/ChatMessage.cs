using System.ComponentModel.DataAnnotations;
using Ukrainians.Infrastrusture.Data.Entities.Base;

namespace Ukrainians.Infrastrusture.Data.Entities
{
    public class ChatMessage : BaseEntity
    {
        [Required]
        public DateTime Created { get; set; }
        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public byte[]? Picture { get; set; }

        [Required]
        [StringLength(50)]
        public string From { get; set; }
        [StringLength(50)]
        public string? To { get; set; }
        [Required]
        public bool Unread { get; set; }

        public Guid? ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
