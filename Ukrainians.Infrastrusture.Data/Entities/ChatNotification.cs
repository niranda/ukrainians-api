using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ukrainians.Infrastrusture.Data.Entities.Base;

namespace Ukrainians.Infrastrusture.Data.Entities
{
    public class ChatNotification : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        public int UnreadMessages { get; set; }

        public Guid? ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
