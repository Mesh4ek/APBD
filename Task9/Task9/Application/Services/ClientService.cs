using Task9.Application.Exceptions;
using Task9.Application.Repository;
using Task9.Application.Services.Interfaces;

namespace Task9.Application.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var clientExists = await clientRepository.ClientExistsAsync(idClient);
        if (!clientExists) 
            return false;
        
        var clientHasTrips = await clientRepository.ClientHasTripsAsync(idClient);
        if (clientHasTrips)
            throw new ClientExceptions.ClientHasTripsException();
        
        return await clientRepository.DeleteClientAsync(idClient);
    }
}