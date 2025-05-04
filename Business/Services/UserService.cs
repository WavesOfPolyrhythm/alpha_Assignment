using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace Business.Services;

/// <summary>
/// UpdateUserRole, This method was partly created with help from ChatGPT.
/// Updates a user's role by first checking that the user exists and the new role is valid.
/// Then it removes any current roles from the user and assigns the new selected role (Admin or User).
/// Returns a result indicating success or failure.
/// </summary>


public interface IUserService
{
    Task<UserResult> GetUsersAsync();
    Task<UserResult> AddUserToRole(string userId, string roleName);
    Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User");
    Task<UserResult> UpdateUserRoleAsync(string userId, string newRole);
}

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<UserResult> GetUsersAsync()
    {
        var result = await _userRepository.GetAllAsync();
        return result.MapTo<UserResult>();
    }

    public async Task<UserResult> AddUserToRole(string userId, string roleName)
    {

        if (!await _roleManager.RoleExistsAsync(roleName))
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "Role does not exists." };

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "User does not exists." };

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded
            ? new UserResult { Succeeded = true, StatusCode = 200 }
            : new UserResult { Succeeded = false, StatusCode = 500, Error = "Unable to add user to role." };
    }

    public async Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User")
    {
        if (formData == null)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Form data can't be null" };

        var existsResult = await _userRepository.ExistsAsync(x => x.Email == formData.Email);
        if (existsResult.Succeeded)
            return new UserResult { Succeeded = false, StatusCode = 409, Error = "User with same email already exists" };

        try
        {
            var userEntity = formData.MapTo<UserEntity>();
            userEntity.UserName = formData.Email;
            var result = await _userManager.CreateAsync(userEntity, formData.Password!);
            if (result.Succeeded)
            {
                var addToRoleResult = await AddUserToRole(userEntity.Id, roleName);
                return result.Succeeded
                   ? new UserResult { Succeeded = true, StatusCode = 201 }
                   : new UserResult { Succeeded = false, StatusCode = 201, Error = "User Created but not added to role" };
            }

            return new UserResult { Succeeded = false, StatusCode = 500, Error = "Unable to create user." };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<UserResult> UpdateUserRoleAsync(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new UserResult { Succeeded = false, StatusCode = 404, Error = "User not found." };

        if (!await _roleManager.RoleExistsAsync(newRole))
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Role does not exist." };

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
            return new UserResult { Succeeded = false, StatusCode = 500, Error = "Failed to remove existing roles." };

        var addResult = await _userManager.AddToRoleAsync(user, newRole);
        if (!addResult.Succeeded)
            return new UserResult { Succeeded = false, StatusCode = 500, Error = "Failed to assign new role." };

        return new UserResult { Succeeded = true, StatusCode = 200 };
    }

}
