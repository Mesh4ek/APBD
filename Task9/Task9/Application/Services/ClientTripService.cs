using Task9.Application.DTOs;
using Task9.Application.Exceptions;
using Task9.Application.Repository;
using Task9.Application.Services.Interfaces;
using Task9.Core.Models;

namespace Task9.Application.Services;

public class ClientTripService(
    IClientRepository clientRepository,
    ITripRepository tripRepository,
    IClientTripRepository? clientTripRepository = null
) : IClientTripService
{
    public async Task RegisterClientToTripAsync(int idTrip, RegisterClientDto dto)
    {
        var existingClient = await clientRepository.GetByPeselAsync(dto.Pesel);

        if (existingClient != null)
        {
            bool clientOnThatTrip = clientTripRepository is not null
                ? await clientTripRepository.ClientTripExistsAsync(existingClient.IdClient, idTrip)
                : await tripRepository.IsClientOnTripAsync(existingClient.IdClient, idTrip);

            if (clientOnThatTrip)
                throw new ClientTripExceptions.ClientAlreadyRegisteredException();
        }
        else
        {
            existingClient = new Client
            {
                FirstName = dto.FirstName,
                LastName  = dto.LastName,
                Email     = dto.Email,
                Telephone = dto.Telephone,
                Pesel     = dto.Pesel
            };

            existingClient = await clientRepository.CreateClientAsync(existingClient);
        }
        
        var trip = await tripRepository.GetByIdAsync(idTrip);
        if (trip == null)
            throw new ClientTripExceptions.TripNotFoundException();
        
        if (trip.DateFrom <= DateTime.UtcNow)
            throw new ClientTripExceptions.TripAlreadyStartedException();
        
        bool alreadyOnTrip = clientTripRepository is not null
            ? await clientTripRepository.ClientTripExistsAsync(existingClient.IdClient, idTrip)
            : await tripRepository.IsClientOnTripAsync(existingClient.IdClient, idTrip);

        if (alreadyOnTrip)
            throw new ClientTripExceptions.ClientAlreadyRegisteredException();
        
        var clientTrip = new ClientTrip
        {
            IdClient     = existingClient.IdClient,
            IdTrip       = idTrip,
            RegisteredAt = DateTime.UtcNow,
            PaymentDate  = dto.PaymentDate
        };

        if (clientTripRepository is not null)
            await clientTripRepository.AddAsync(clientTrip);
        else
            await tripRepository.AddClientTripAsync(clientTrip);
    }
}
