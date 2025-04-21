using System.ComponentModel.DataAnnotations;
namespace WebApp.Models;

public class LogInViewModel
{
    [Required(ErrorMessage = "You must enter your email address")]
    [EmailAddress(ErrorMessage = "You must enter a valid email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "You must enter your password")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    [Display(Name = "Password", Prompt = "Enter password")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
