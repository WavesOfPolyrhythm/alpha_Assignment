using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
namespace WebApp.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        var viewModel = new ProjectsViewModel()
        {
            Projects = SetProjects()
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Index(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new ProjectsViewModel
            {
                Projects = SetProjects(),
                AddProjectFormData = model,
                EditProjectFormData = new EditProjectViewModel()
            };
            return View(viewModel);
        }

        return RedirectToAction("Index");
    }


    private IEnumerable<ProjectViewModel> SetProjects()
    {
        var projects = new List<ProjectViewModel>();

        projects.Add(new ProjectViewModel
        {
            Id = Guid.NewGuid().ToString(),
            ProjectName = "New Website",
            Company = "ABC Data",
            Description = "Build a new website for ABC Data"
        });

        return projects;
    }

}
