using Ukrainians.UtilityServices.Common.Enums;

namespace Ukrainians.UtilityServices.Models.Auth
{
    public class AuthResultModel
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public ErrorCode ErrorMessage { get; set; }
    }
}
