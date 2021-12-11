using Microsoft.AspNetCore.Mvc;

namespace Unicorn.Core.Services.Authentication.OpenIddict.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Error()
    {
        return View("~/Views/Shared/Error.cshtml");
    }
}
