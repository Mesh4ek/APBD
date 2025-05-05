using Task7.Models;

namespace Task7.Repositories.Interfaces;

public interface IClientTripRepository
{
    Task<IEnumerable<ClientTrip>> GetAllByClientIdAsync(int clientId);
    Task AddAsync(ClientTrip clientTrip);
    Task<bool> ExistsAsync(int clientId, int tripId);
    Task RemoveAsync(int clientId, int tripId);
}