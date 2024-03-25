namespace Ukrainians.UtilityServices.Models.Common
{
    public class ChatModel
    {
        public string ChatMessage { get; set; }
        public Guid PrivateChatId { get; set; }
        public string User { get; set; }
        public int Unread { get; set; }
    }
}
