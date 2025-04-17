using Data.Contexts;
using Data.Entities;
using Domain.Models;
namespace Data.Repositories;

public interface IStatusRepository : IBaseRepository<StatusEntity, Status>
{

}
public class StatusRepository(AlphaDbContext context) : BaseRepository<StatusEntity, Status>(context), IStatusRepository
{

}
