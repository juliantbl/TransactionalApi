using System.Linq.Expressions;
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;

namespace TransactionalDomain.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetClients();
        Task<bool> ClientExists(string identification);
        Task AddUserClient(ClientDto clientDto);
        Task<Client> GetClientById(int id);
        Task RemoveClient(Client client);
        Task<Client> UpdateClient(int id, ClientDto clientDto, bool replace);
    }
}
