using System.Security;
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;
using TransactionalDomain.Interfaces;

namespace TransactionalBll.Services
{
    public class ClientService : IClientService
    {
        IRepositoryAsync<Client> repository;
        IRepositoryAsync<User> userRepository;

        public ClientService(IRepositoryAsync<Client> _repository, IRepositoryAsync<User> _userRepository)
        {
            repository = _repository;
            userRepository = _userRepository;
        }
        public async Task<IEnumerable<Client>> GetClients()
        {
          return await repository.Get();
        }

        public async Task<bool> ClientExists(string identification)
        {
            var client= await repository.Find(x=>x.User.Identification== identification);
            return client!=null;
        }

        public async Task AddUserClient(ClientDto clientDto)
        {
            var newUser = new User {
                Address = clientDto.Address,
                Age = clientDto.Age,
                Gender = clientDto.Gender,
                Identification = clientDto.Identification,
                Name= clientDto.Name,
                PhoneNumber=clientDto.PhoneNumber
            };

            await userRepository.Insert(newUser);

            var newClient = new Client 
            {
                Password=clientDto.Password,
                Status = true,
                UserId= newUser.Id
            };

            await repository.Insert(newClient);
        }

        public async Task<Client> GetClientById(int id)
        {
            return await repository.GetById(id);
        }

        public async Task RemoveClient(Client client)
        {
            await repository.Delete(client.Id);
            await userRepository.Delete(client.User.Id);
        }

        public async Task<Client> UpdateClient(int id, ClientDto clientDto, bool replace) 
        { 
            var currentClient=await repository.GetById(id);

            if (currentClient == null) return null;

            var currentUser= currentClient.User;

            if (replace || !string.IsNullOrEmpty(clientDto.Address))
                currentUser.Address = clientDto.Address;

            if (replace || clientDto.Age>0)
                currentUser.Age = clientDto.Age;

            if (replace || !string.IsNullOrEmpty(clientDto.Gender))
                currentUser.Gender = clientDto.Gender;

            if (replace || !string.IsNullOrEmpty(clientDto.Identification))
                currentUser.Identification = clientDto.Identification;

            if (replace || !string.IsNullOrEmpty(clientDto.Name))
                currentUser.Name = clientDto.Name;

            if (replace || !string.IsNullOrEmpty(clientDto.PhoneNumber))
                currentUser.PhoneNumber = clientDto.PhoneNumber;

            if (replace || !string.IsNullOrEmpty(clientDto.Password))
                currentClient.Password = clientDto.Password;

            if (replace || clientDto.Status!= currentClient.Status)
                currentClient.Status = clientDto.Status;

            await userRepository.Update(currentUser);

            await repository.Update(currentClient);

            return currentClient;
        } 

    }
}
