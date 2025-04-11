using System.ComponentModel.DataAnnotations;
namespace WebApp.Models;
public class SignUpViewModel
{
    [Required(ErrorMessage = "You must enter your full name")]
    [Display(Name = "Full Name", Prompt = "Enter full name")]
    [DataType(DataType.Text)]
    public string FullName { get; set; } = null!;

    [Required]
    [Display(Name = "Email", Prompt = "Enter email")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "You must enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Your password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Confirm Password", Prompt = "Confirm password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Range(typeof(bool), "true", "true")]
    public bool Terms { get; set; }
}
