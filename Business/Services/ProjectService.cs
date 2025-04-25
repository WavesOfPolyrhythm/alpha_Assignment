using Azure;
using Domain.Dtos;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult<Project>> GetProjectAsync(string Id);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {

        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied." };

        var project = new ProjectEntity
        {
            ProjectName = formData.ProjectName,
            Description = formData.Description,
            StartDate = formData.StartDate ?? DateTime.Now,
            EndDate = formData.EndDate ?? DateTime.Now,
            Budget = formData.Budget,
            ClientId = formData.ClientId,
            UserId = formData.UserId,
            StatusId = "1",
        };

        var result = await _projectRepository.AddAsync(project);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (orderByDescending: true,
                sortBy: s => s.Created, where: null,
                include => include.User,
                include => include.Status,
                include => include.Client
            );

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string Id)
    {
        var response = await _projectRepository.GetAsync
            (
                where: x => x.Id == Id,
                include => include.User,
                include => include.Status,
                include => include.Client
            );
        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{Id}' was not found." };
    }
}
