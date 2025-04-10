using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
namespace WebApp.Controllers;

public class AccountController : Controller
{


    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid) 
            return View(model);
        return View();
    }

    public IActionResult SignOut()
    {
        return View();
    }
    public IActionResult LogIn()
    {
        return View();
    }
}
