using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class UserRoleViewModel
{
    [Required(ErrorMessage = "You must select a user")]
    [Display(Name = "User", Prompt = "Choose user")]
    public string UserId { get; set; } = null!;

    [Required(ErrorMessage = "You must select a role")]
    [Display(Name = "Authorisation", Prompt = "Choose role")]
    public string Role { get; set; } = null!;

    public IEnumerable<SelectListItem> Users { get; set; } = [];
    public IEnumerable<SelectListItem> Roles { get; set; } = [];
}
