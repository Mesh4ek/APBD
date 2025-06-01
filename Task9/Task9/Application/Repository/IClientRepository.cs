using Task9.Core.Models;

namespace Task9.Application.Repository;

public interface IClientRepository
{ 
    Task<bool> ClientExistsAsync(int idClient);
    Task<bool> ClientHasTripsAsync(int idClient);
    Task<bool> DeleteClientAsync(int idClient);
    Task<Client?> GetByPeselAsync(string pesel);
    Task<Client> CreateClientAsync(Client client);
}