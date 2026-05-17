// Services/IEmailService.cs

namespace Portafolio.Services.Mails;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}