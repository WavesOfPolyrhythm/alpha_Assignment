using Business.Services;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
namespace WebApp.Controllers;

public class AccountController(IAccountService accountService) : Controller
{

    private readonly IAccountService _accountService = accountService;

    [Route("account/signup")]
    public IActionResult SignUp()
    {
        var model = new SignUpViewModel();
        return View(model);
    }

    [HttpPost]
    [Route("account/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        ViewBag.ErrorMessage = null!;

        if (!ModelState.IsValid) 
            return View(model);

        var signUpFormData = model.MapTo<SignUpFormData>();
        var result = await _accountService.SignUpAsync(signUpFormData);
        if (result.Succeeded)
        {
            return RedirectToAction("LogIn", "Account");
        }

        ViewBag.ErrorMessage = result.Error;
        return View(model);
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
