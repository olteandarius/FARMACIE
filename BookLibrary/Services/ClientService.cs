using BookLibrary.DAL;
using BookLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Services
{
    public interface IClientService
    {
        Task<List<Client>> List();
        Task Remove(Guid id);
        Task Insert(Client book);
        Task Update(Client book);
        Task<Client> GetById(Guid? id);
    }

    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _clientRepository;

        public ClientService(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<List<Client>> List()
        {
            return await _clientRepository.List();
        }

        public async Task Remove(Guid id)
        {
            await _clientRepository.Remove(id);
            await _clientRepository.Save();
        }

        public async Task Insert(Client client)
        {
            client.Id = Guid.NewGuid();
            await _clientRepository.Insert(client);
            await _clientRepository.Save();
        }

        public async Task Update(Client client)
        {
            _clientRepository.Update(client);
            await _clientRepository.Save();
        }

        public async Task<Client> GetById(Guid? id)
        {
            if (id == null)
            {
                return null;
            }

            return await _clientRepository.GetById(id.Value);
        }
    }
}
