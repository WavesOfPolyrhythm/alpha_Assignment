using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models;

public class AddProjectViewModel
{
    public IEnumerable<SelectListItem> Statuses { get; set; } = [];
}
