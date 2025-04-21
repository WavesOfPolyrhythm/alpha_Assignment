using Business.Services;
using Data.Entities;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
namespace WebApp.Controllers;

public class AccountController(IAccountService accountService, SignInManager<UserEntity> signInManager) : Controller
{

    private readonly IAccountService _accountService = accountService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

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
    public async Task<IActionResult> LogIn(LogInViewModel model)
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
            return RedirectToAction("Index", "Admin");

        ViewBag.ErrorMessage = "Invalid email or password.";
        return View(model);
    }


    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("LogIn", "Account");
    }
}
