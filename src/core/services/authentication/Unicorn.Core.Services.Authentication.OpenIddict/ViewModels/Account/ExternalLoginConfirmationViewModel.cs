using System.ComponentModel.DataAnnotations;

namespace Unicorn.Core.Services.Authentication.OpenIddict.ViewModels.Account;

public class ExternalLoginConfirmationViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
