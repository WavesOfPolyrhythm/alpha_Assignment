namespace Domain.Models;

public class User
{
    public string Id { get; set; } = null!;
    public string? FullName { get; set; }
    public string? JobTitle { get; set; }
    public string Email { get; set; } = null!;
    public string? Phonenumber { get; set; }
}
