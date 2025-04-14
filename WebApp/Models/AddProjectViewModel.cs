using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class AddProjectViewModel
{
    [Required(ErrorMessage = "You must enter a project name")]
    [Display(Name = "Project Name", Prompt = "Project name")]
    public string ProjectName { get; set; } = null!;

    [Required(ErrorMessage = "You must choose a client")]
    [Display(Name = "Clients", Prompt = "Clients")]
    public string ClientId { get; set; } = null!;

    public IEnumerable<SelectListItem> Clients { get; set; } = [];

    [Required(ErrorMessage = "You need to add a description")]
    [Display(Name = "Description", Prompt = "Description")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Startdate is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "You need to enter a budget")]
    [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive number")]
    [Display(Name = "Budget", Prompt = "Budget")]
    public decimal? Budget {  get; set; }
}
