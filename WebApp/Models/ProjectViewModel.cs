namespace WebApp.Models;

public class ProjectViewModel
{
    public string Id { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public string Company { get; set; } = null!;
    public string? Description { get; set; } = null!;

    public string ClientName { get; set; } = null!;
    public string Status { get; set; } = null!;
}
