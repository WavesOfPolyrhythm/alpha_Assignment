using Business.Services;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.Models;
namespace WebApp.Controllers;

public class AdminController(IProjectService projectService, IClientService clientService, IStatusService statusService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;
    private readonly IStatusService _statusService = statusService;


    [HttpGet]
    [Route("admin/projects")]
    public async Task<IActionResult> Index()
    {
        var clients = await SetClients();
        var statuses = await SetStatus();

        var viewModel = new ProjectsViewModel()
        {
            Projects = await SetProjects(),

            AddProjectFormData = new AddProjectViewModel
            {
                Clients = clients,
            },

            EditProjectFormData = new EditProjectViewModel
            {
                Clients = clients,
                Statuses = statuses
            }
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("admin/add")]
    public async Task<IActionResult> AddProject(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                );

            return BadRequest(new { success = false, errors });
        }

        var formData = model.MapTo<AddProjectFormData>();
        formData.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!; //Code-snippet by CHAT-GPT
        var result = await _projectService.CreateProjectAsync(formData);
        if (result.Succeeded)
        {
            return Ok(new { success = true });
        }
        else
        {
            return Problem("Unable to submit data");
        }
    }

    [HttpPost]
    [Route("admin/edit")]
    public async  Task<IActionResult> EditProject(EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Clients = await SetClients();
            model.Statuses = await SetStatus();

            var viewModel = new ProjectsViewModel()
            {
                Projects = await SetProjects(),
                AddProjectFormData = new AddProjectViewModel
                {
                    Clients = await SetClients(),
                },

                EditProjectFormData = model
            };

            ViewData["ShowEditModal"] = true;
            return View("Index", viewModel);
        }

        return RedirectToAction("Index");
    }


    private async Task<IEnumerable<ProjectViewModel>> SetProjects()
    {
        var result = await _projectService.GetProjectsAsync();
        if (result.Succeeded && result.Result != null)
        {
            return result.Result.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Description = p.Description,
                Company = p.Client.ClientName,
                Status = p.Status.StatusName
            });
        }

        return [];
    }

    private async Task<IEnumerable<SelectListItem>> SetClients()
    {
        var result = await _clientService.GetClientsAsync();
        var list = result.Result;

        if (list == null)
            return [];

       var clients = list.Select(c => new SelectListItem
        {
            Value = c.Id,
            Text = c.ClientName
        });

        return clients;
    }

    private async Task<IEnumerable<SelectListItem>> SetStatus()
    {
        var result = await _statusService.GetStatusesAsync();
        var list = result.Result;

        if (list == null)
            return [];

        var statuses = list.Select(s => new SelectListItem
        {
            Value = s.Id,
            Text = s.StatusName
        });

        return statuses;
    }
}
