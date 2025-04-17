using Data.Contexts;
using Data.Entities;
using Domain.Models;
namespace Data.Repositories;

public interface IClientRepository : IBaseRepository<ClientEntity, Client>
{

}

public class ClientRepository(AlphaDbContext context) : BaseRepository<ClientEntity, Client>(context), IClientRepository
{

}
