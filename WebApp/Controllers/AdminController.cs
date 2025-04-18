﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
namespace WebApp.Controllers;

public class AdminController : Controller
{
    [HttpGet]
    [Route("admin/projects")]
    public IActionResult Index()
    {
        var viewModel = new ProjectsViewModel()
        {
            Projects = SetProjects(),
            AddProjectFormData = new AddProjectViewModel
            {
                Clients = SetClients(),
            },

            EditProjectFormData = new EditProjectViewModel
            {
                Clients = SetClients(),
                Statuses = SetStatus()
            }
        };
        return View(viewModel);
    }

    [HttpPost]
    [Route("admin/add")]
    public IActionResult AddProject(AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Clients = SetClients();
            var viewModel = new ProjectsViewModel()
            {
                Projects = SetProjects(),
                AddProjectFormData = model,
                EditProjectFormData = new EditProjectViewModel
                {
                    Clients = SetClients(),
                    Statuses = SetStatus()
                }
            };

            ViewData["ShowAddModal"] = true;
            return View("Index", viewModel);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("admin/edit")]
    public IActionResult EditProject(EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Clients = SetClients();
            model.Statuses = SetStatus();
            var viewModel = new ProjectsViewModel()
            {
                Projects = SetProjects(),
                AddProjectFormData = new AddProjectViewModel
                {
                    Clients = SetClients(),
                },
                EditProjectFormData = model
            };

            viewModel.EditProjectFormData.Statuses = SetStatus();

            ViewData["ShowEditModal"] = true;
            return View("Index", viewModel);
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
            Description = "Build a new website for ABC Data",
            ClientName = "EPN Sverige AB",
            Status = "STARTED"
        });

        return projects;
    }

    private IEnumerable<SelectListItem> SetClients()
    {
        var clients = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "EPN Sverige AB"},
            new SelectListItem { Value = "2", Text = "Etab Data"},
            new SelectListItem { Value = "3", Text = "DesignNow"},
        };

        return clients;
    }

    private IEnumerable<SelectListItem> SetStatus()
    {
        var statuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "STARTED", Selected = true },
            new SelectListItem { Value = "2", Text = "COMPLETED" },
        };

        return statuses;
    }
}
