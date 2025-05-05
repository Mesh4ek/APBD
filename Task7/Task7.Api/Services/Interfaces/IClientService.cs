using Task7.Contracts.Requests;
using Task7.Contracts.Responses;

namespace Task7.Services.Interfaces;

public interface IClientService
{
    Task<IEnumerable<GetAllClientTripsResponse>> GetAllClientTripsAsync(int clientId);
    Task<int> CreateClientAsync(CreateClientRequest request);
    Task RegisterClientToTripAsync(int clientId, int tripId);
    Task RemoveClientFromTripAsync(int clientId, int tripId);
}