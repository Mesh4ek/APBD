using Microsoft.EntityFrameworkCore;
using Task9.Application.Repository;
using Task9.Core.Models;

namespace Task9.Infrastructure.Repository;

public class ClientTripRepository(Task9Context context) : IClientTripRepository
{
    public async Task<bool> ClientTripExistsAsync(int clientId, int tripId)
    {
        return await context.ClientTrips
            .AnyAsync(ct => ct.IdClient == clientId && ct.IdTrip == tripId);
    }
    
    public async Task AddAsync(ClientTrip clientTrip)
    {
        context.ClientTrips.Add(clientTrip);
        await context.SaveChangesAsync();
    }
}