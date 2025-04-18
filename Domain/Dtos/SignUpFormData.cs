namespace Domain.Dtos;

public class SignUpFormData
{
    public string? FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Password { get; set; } = null!;
}
