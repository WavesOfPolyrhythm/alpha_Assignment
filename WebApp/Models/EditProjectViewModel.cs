using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models;

public class EditProjectViewModel
{
    public IEnumerable<SelectListItem> Statuses { get; set; } = [];
}
