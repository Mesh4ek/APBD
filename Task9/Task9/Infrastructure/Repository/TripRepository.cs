using Microsoft.EntityFrameworkCore;
using Task9.Application.Repository;
using Task9.Core.Models;

namespace Task9.Infrastructure.Repository;

public class TripRepository(Task9Context tripDbContext) : ITripRepository
{
    public async Task<PaginatedResult<Core.Models.Trip>> GetPaginatedTripsAsync(int page = 1, int pageSize = 10)
    {
        var tripsQuery = tripDbContext.Trips
            .Include(e => e.ClientTrips).ThenInclude(e => e.IdClientNavigation)
            .Include(e => e.IdCountries)
            .OrderByDescending(e => e.DateFrom);

        var tripsCount = await tripsQuery.CountAsync();
        var totalPages = tripsCount / pageSize;
        var trips = await tripsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<Core.Models.Trip>
        {
            PageSize = pageSize,
            PageNum = page,
            AllPages = totalPages,
            Data = trips
        };
    }

    public async Task<List<Core.Models.Trip>> GetAllTripsAsync()
    {
        return await tripDbContext.Trips
            .Include(e => e.ClientTrips).ThenInclude(e => e.IdClientNavigation)
            .Include(e => e.IdCountries)
            .OrderBy(e => e.DateFrom)
            .ToListAsync();
    }
    
    public async Task<Trip?> GetByIdAsync(int idTrip)
    {
        return await tripDbContext.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .FirstOrDefaultAsync(t => t.IdTrip == idTrip);
    }
    
    public async Task<bool> IsClientOnTripAsync(int clientId, int tripId)
    {
        return await tripDbContext.ClientTrips
            .AnyAsync(ct => ct.IdClient == clientId && ct.IdTrip == tripId);
    }
    
    public async Task AddClientTripAsync(ClientTrip clientTrip)
    {
        tripDbContext.ClientTrips.Add(clientTrip);
        await tripDbContext.SaveChangesAsync();
    }
}