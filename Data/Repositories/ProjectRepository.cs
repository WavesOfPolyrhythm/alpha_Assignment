using Data.Contexts;
using Data.Entities;
using Domain.Models;
namespace Data.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{

}
public class ProjectRepository(AlphaDbContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{

}
