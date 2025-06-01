using Task9.Core.Models;

namespace Task9.Application.Repository;

public interface ITripRepository
{
    Task<PaginatedResult<Core.Models.Trip>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10);
    Task<List<Core.Models.Trip>> GetAllTripsAsync();
    Task<Trip?> GetByIdAsync(int idTrip);
    Task<bool> IsClientOnTripAsync(int clientId, int tripId);
    Task AddClientTripAsync(ClientTrip clientTrip);
}