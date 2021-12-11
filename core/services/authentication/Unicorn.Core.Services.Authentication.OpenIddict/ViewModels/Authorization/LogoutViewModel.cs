using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Unicorn.Core.Services.Authentication.OpenIddict.ViewModels.Authorization;

public class LogoutViewModel
{
    [BindNever]
    public string RequestId { get; set; }
}
