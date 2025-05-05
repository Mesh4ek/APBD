using Task7.Contracts.Requests;
using Task7.Contracts.Responses;

namespace Task7.Services.Interfaces;

public interface ITripService
{
    Task<IEnumerable<GetAllTripsResponse>> GetAllTripsAsync();
    Task<GetAllTripsResponse?> GetTripByIdAsync(int id);
}