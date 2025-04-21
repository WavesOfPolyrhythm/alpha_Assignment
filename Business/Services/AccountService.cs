using Domain.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Business.Models;

namespace Business.Services;

public interface IAccountService
{
    Task<AccountResult> SignInAsync(LogInFormData formData);
    Task<AccountResult> SignOutasync();
    Task<AccountResult> SignUpAsync(SignUpFormData formData);
}

public class AccountService(IUserService userService, SignInManager<UserEntity> signInManager) : IAccountService
{
    private readonly IUserService _userService = userService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<AccountResult> SignInAsync(LogInFormData formData)
    {
        if (formData == null)
            return new AccountResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied." };

        var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, formData.RememberMe, false);
        return result.Succeeded
          ? new AccountResult { Succeeded = true, StatusCode = 200 }
          : new AccountResult { Succeeded = false, StatusCode = 401, Error = "Invalid email or password" };
    }

    public async Task<AccountResult> SignUpAsync(SignUpFormData formData)
    {
        if (formData == null)
            return new AccountResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied." };

        var result = await _userService.CreateUserAsync(formData);
        return result.Succeeded
           ? new AccountResult { Succeeded = true, StatusCode = 201 }
           : new AccountResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<AccountResult> SignOutasync()
    {
        await _signInManager.SignOutAsync();
        return new AccountResult { Succeeded = true, StatusCode = 200 };
    }
}