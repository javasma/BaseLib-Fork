using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using BaseLib.Core.Mail;
using BaseLib.Core.Models;
using MimeKit;

namespace BaseLib.Core.AmazonCloud.Mail;

public class AmazonEmailSender: IEmailSender
{
    private readonly IAmazonSimpleEmailServiceV2 emailService;

    public AmazonEmailSender(IAmazonSimpleEmailServiceV2 emailService)
    {
        this.emailService = emailService;
    }
    public async Task<EmailResponse> SendAsync(MimeMessage message)
    {
        var sendEmailRequest = MapToAmazonRequest(message);
        try
        {
            var emailResponse = await this.emailService.SendEmailAsync(sendEmailRequest);
            return new EmailResponse
            {
                Succeeded = emailResponse.HttpStatusCode == System.Net.HttpStatusCode.OK
            };
        }
        catch (MessageRejectedException e)
        {
            return new EmailResponse
            {
                Succeeded = false,
                ReasonCode = CoreReasonCode.Failed,
                Messages = new string[] { e.ToString() }
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static SendEmailRequest MapToAmazonRequest(MimeMessage message)
    {

        var destination = new Destination()
        {
            ToAddresses = message.To.Select(x => ((MailboxAddress)x).Address).ToList(),
        };
        if (message.Cc != null && message.Cc.Any())
        {
            destination.CcAddresses = message.Cc.Select(x => ((MailboxAddress)x).Address).ToList();
        }
        if (message.Bcc != null && message.Bcc.Any())
        {
            destination.BccAddresses = message.Bcc.Select(x => ((MailboxAddress)x).Address).ToList();
        }
        using (var messageStream = new MemoryStream())
        {
            message.WriteTo(messageStream);
            messageStream.Position = 0;
            var emailRequest = new SendEmailRequest
            {
                FromEmailAddress = message.From.Select(x => (MailboxAddress)x)?.FirstOrDefault()?.Address,
                Destination = destination,
                Content = new EmailContent
                {
                    Raw = new RawMessage
                    {
                        Data = messageStream
                    }
                }
            };
            return emailRequest;
        }
    }
}
