using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    public string? FullName { get; set; }
    public string? JobTitle { get; set; }
    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
