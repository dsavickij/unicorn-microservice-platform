using System.Threading.Tasks;

namespace Unicorn.Core.Services.Authentication.OpenIddict.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
