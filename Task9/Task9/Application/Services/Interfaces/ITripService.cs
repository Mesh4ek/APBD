using Task9.Application.DTOs;
using Task9.Core.Models;

namespace Task9.Application.Services.Interfaces;

public interface ITripService
{
    Task<PaginatedResult<GetTripDto>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10);
    Task<List<GetTripDto>> GetAllTripsAsync();
}