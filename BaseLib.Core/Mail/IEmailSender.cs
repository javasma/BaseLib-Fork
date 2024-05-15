using BaseLib.Core.Models;
using MimeKit;

namespace BaseLib.Core.Mail;
public interface IEmailSender
{
    Task<EmailResponse> SendAsync(MimeMessage message);
}

public class EmailResponse : CoreResponseBase
{

}