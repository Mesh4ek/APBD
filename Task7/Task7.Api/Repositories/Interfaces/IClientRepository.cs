using Task7.Models;

namespace Task7.Repositories.Interfaces;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<Client?> GetByPeselAsync(string pesel);
    Task AddAsync(Client client);
    Task<bool> ExistsAsync(int id);
}