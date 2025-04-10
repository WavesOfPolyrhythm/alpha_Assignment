using System.ComponentModel.DataAnnotations;
namespace WebApp.Models;
public class SignUpViewModel
{
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Full Name", Prompt = "Enter full name")]
    public string FullName { get; set; } = null!;

    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "You must enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter password")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Your password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm password")]
    public string ConfirmPassword { get; set; } = null!;

    [Range(typeof(bool), "true", "true")]
    public bool Terms { get; set; }
}
