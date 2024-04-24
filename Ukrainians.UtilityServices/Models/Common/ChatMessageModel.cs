using Microsoft.AspNetCore.Http;
using Ukrainians.UtilityServices.Models.Base;

namespace Ukrainians.UtilityServices.Models.Common
{
    public class ChatMessageModel : BaseModel
    {
        public DateTime Created { get; set; }
        public string Content { get; set; }
        public string From { get; set; }
        public string? To { get; set; }
        public string? Picture { get; set; }
        public bool Unread { get; set; }
    }
}
