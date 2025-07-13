using System.Threading.Tasks;

namespace Unicorn.Core.Services.Authentication.OpenIddict.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}