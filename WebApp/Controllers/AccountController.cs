using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
namespace WebApp.Controllers;

public class AccountController : Controller
{

    [Route("account/signup")]
    public IActionResult SignUp()
    {
        var model = new SignUpViewModel();
        return View(model);
    }

    [HttpPost]
    [Route("account/signup")]
    public IActionResult SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid) 
            return View(model);
        return View();
    }

    [Route("account/login")]
    public IActionResult LogIn()
    {
        return View();
    }

    [HttpPost]
    [Route("account/login")]
    public IActionResult LogIn(LogInViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        return View();
    }


    public IActionResult SignOut()
    {
        return View();
    }
}
