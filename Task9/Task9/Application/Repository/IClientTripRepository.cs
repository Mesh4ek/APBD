using Task9.Core.Models;

namespace Task9.Application.Repository;

public interface IClientTripRepository
{
    Task<bool> ClientTripExistsAsync(int clientId, int tripId);
    Task AddAsync(ClientTrip clientTrip);
}