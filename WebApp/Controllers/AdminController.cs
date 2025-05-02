using Business.Services;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.Handlers;
using WebApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WebApp.Controllers;

[Authorize]
public class AdminController(IProjectService projectService, IClientService clientService, IStatusService statusService, IFileHandler fileHandler) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;
    private readonly IStatusService _statusService = statusService;
    private readonly IFileHandler _fileHandler = fileHandler;

    // Projects Count was made with help from ChatGPT.
    // It calculates the total number of projects, as well as the number of started and completed projects.
    // This makes it possible to show the correct count next to the filter tabs on the project page.

    [HttpGet]
    [Route("admin/projects")]
    public async Task<IActionResult> Index()
    {
        var clients = await SetClients();
        var statuses = await SetStatus();
        var projects = await SetProjects();

        var viewModel = new ProjectsViewModel()
        {
            Projects = projects,
            AllCount = projects.Count(),
            StartedCount = projects.Count(p => p.Status == "STARTED"),
            CompletedCount = projects.Count(p => p.Status == "COMPLETED"),

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

    //Code-snippet by CHAT-GPT, "FindFirstValue"
    // This line sets the UserId in formData to the ID of the currently logged-in user.
    // It uses the NameIdentifier claim to get the user's unique ID from the login session.

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
        formData.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (model.Image != null && model.Image.Length > 0)
        {
            var imageUri = await _fileHandler.UploadAsync(model.Image);
            formData.Image = imageUri;
        }

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
    public async Task<IActionResult> EditProject(EditProjectViewModel model)
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

        var formData = model.MapTo<EditProjectFormData>();
        formData.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _projectService.UpdateProjectAsync(formData);
        if (result.Succeeded)
        {
            return Ok(new { success = true });
        }
        else
        {
            return Problem("Unable to submit data");
        }
    }

    [HttpGet]
    [Route("admin/delete")]
    public async Task<IActionResult> DeleteProject(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest(new { success = false, error = "Invalid project Id." });

        var result = await _projectService.DeleteProjectAsync(id);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Admin");
        }
        else
        {
            return Problem("Unable to delete project");
        }

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
