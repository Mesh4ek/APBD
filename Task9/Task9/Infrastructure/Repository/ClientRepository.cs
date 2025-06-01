using Microsoft.EntityFrameworkCore;
using Task9.Application.Repository;
using Task9.Core.Models;

namespace Task9.Infrastructure.Repository;

public class ClientRepository(Task9Context context) : IClientRepository
{
    
    public async Task<bool> ClientExistsAsync(int idClient)
    {
        var client = await context.Clients.FirstOrDefaultAsync(x => x.IdClient == idClient);
        return client is not null;
    }

    public async Task<bool> ClientHasTripsAsync(int idClient)
    {
        return await context.ClientTrips.AnyAsync(ct => ct.IdClient == idClient);
    }

    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var client = await context.Clients.FindAsync(idClient);
        if (client == null)
        {
            return false;
        }

        context.Clients.Remove(client);
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<Client?> GetByPeselAsync(string pesel)
    {
        return await context.Clients.FirstOrDefaultAsync(c => c.Pesel == pesel);
    }
    
    public async Task<Client> CreateClientAsync(Client client)
    {
        context.Clients.Add(client);
        await context.SaveChangesAsync();
        return client;
    }
}