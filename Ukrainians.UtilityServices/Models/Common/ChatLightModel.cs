using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ukrainians.UtilityServices.Models.Common
{
    public class ChatLightModel
    {
        public string ChatMessage { get; set; }
        public Guid PrivateChatId { get; set; }
        public UserModel User { get; set; }
        public int Unread { get; set; }
    }
}
