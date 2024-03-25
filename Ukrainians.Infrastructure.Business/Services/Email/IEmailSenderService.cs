using Ukrainians.UtilityServices.Models.Common;

namespace Ukrainians.Infrastructure.Business.Services.Email
{
    public interface IEmailSenderService
    {
        void SendEmail(MessageModel message);
        Task SendEmailAsync(MessageModel message);
    }
}
