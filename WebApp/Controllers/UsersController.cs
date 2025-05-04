using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
namespace WebApp.Controllers;


[Authorize]
public class UsersController(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager, IUserService userService) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IUserService _userService = userService;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await SetUserRoleViewModelAsync();
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Index(UserRoleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await SetUserRoleViewModelAsync(model));

        var result = await _userService.UpdateUserRoleAsync(model.UserId, model.Role);

        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = "Failed to assign role.";
            return View(await SetUserRoleViewModelAsync(model));
        }

        ViewBag.Message = "Role assigned successfully!";
        return RedirectToAction("Index");
    }


    private async Task<UserRoleViewModel> SetUserRoleViewModelAsync(UserRoleViewModel? model = null)
    {
        var viewModel = model ?? new UserRoleViewModel();

        viewModel.Users = await _userManager.Users.Select(u => new SelectListItem
        {
            Value = u.Id,
            Text = u.Email
        })
        .ToListAsync();

        viewModel.Roles = await _roleManager.Roles.Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        })
        .ToListAsync();

        return viewModel;
    }

}
