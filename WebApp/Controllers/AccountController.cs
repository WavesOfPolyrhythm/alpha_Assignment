using Microsoft.AspNetCore.Mvc;
namespace WebApp.Controllers;

public class AccountController : Controller
{
    public IActionResult LogIn()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

    public IActionResult SignOut()
    {
        return View();
    }
}
