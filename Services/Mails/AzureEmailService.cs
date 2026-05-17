using Azure;
using Azure.Communication.Email;

namespace Portafolio.Services.Mails;

public class AzureEmailService : IEmailService
{
    private readonly EmailClient _emailClient;
    private readonly IConfiguration _configuration;

    public AzureEmailService(IConfiguration configuration)
    {
        _configuration = configuration;

        var connectionString = _configuration["AzureEmail:ConnectionString"];

        _emailClient = new EmailClient(connectionString);
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var sender = _configuration["AzureEmail:SenderAddress"];

        var emailMessage = new EmailMessage(
            senderAddress: sender,
            content: new EmailContent(subject)
            {
                Html = body
            },
            recipients: new EmailRecipients(
                new List<EmailAddress>
                {
                    new(to)
                }
            )
        );

        await _emailClient.SendAsync(
            Azure.WaitUntil.Completed,
            emailMessage
        );
    }
}