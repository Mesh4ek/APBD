using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task7.Contracts.Requests;
using Task7.Contracts.Responses;
using Task7.Models;
using Task7.Repositories.Interfaces;
using Task7.Services.Interfaces;

namespace Task7.Services.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IClientTripRepository _clientTripRepository;
    private readonly ITripRepository _tripRepository;
    private readonly ICountryTripRepository _countryTripRepository;

    public ClientService(
        IClientRepository clientRepository,
        IClientTripRepository clientTripRepository,
        ITripRepository tripRepository,
        ICountryTripRepository countryTripRepository)
    {
        _clientRepository = clientRepository;
        _clientTripRepository = clientTripRepository;
        _tripRepository = tripRepository;
        _countryTripRepository = countryTripRepository;
    }

    public async Task<int> CreateClientAsync(CreateClientRequest request)
    {
        var existing = await _clientRepository.GetByPeselAsync(request.Pesel);
        if (existing != null)
            throw new InvalidOperationException($"A client with PESEL '{request.Pesel}' already exists.");

        var client = new Client
        {
            Pesel = request.Pesel,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate,
            Email = request.Email
        };

        await _clientRepository.AddAsync(client);
        return client.Id;
    }

    public async Task<IEnumerable<GetAllClientTripsResponse>> GetAllClientTripsAsync(int clientId)
    {
        if (!await _clientRepository.ExistsAsync(clientId))
            throw new InvalidOperationException($"Client with ID {clientId} does not exist.");

        var registrations = await _clientTripRepository.GetAllByClientIdAsync(clientId);
        var results = new List<GetAllClientTripsResponse>();

        foreach (var reg in registrations)
        {
            var trip = await _tripRepository.GetByIdAsync(reg.TripId);
            if (trip == null)
                throw new InvalidOperationException($"Trip with ID {reg.TripId} does not exist.");

            var countryLinks = await _countryTripRepository.GetAllByTripIdAsync(reg.TripId);
            var countries = countryLinks
                .Select(ct => new CountryResponse { Id = ct.CountryId, Name = string.Empty })
                .ToList();

            results.Add(new GetAllClientTripsResponse
            {
                TripId = reg.TripId,
                TripName = trip.Name,
                RegisteredAt = reg.RegisteredAt,
                Countries = countries
            });
        }

        return results;
    }

    public async Task RegisterClientToTripAsync(int clientId, int tripId)
    {
        if (!await _clientRepository.ExistsAsync(clientId))
            throw new InvalidOperationException($"Client with ID {clientId} does not exist.");
        if (!await _tripRepository.ExistsAsync(tripId))
            throw new InvalidOperationException($"Trip with ID {tripId} does not exist.");
        if (await _clientTripRepository.ExistsAsync(clientId, tripId))
            throw new InvalidOperationException($"Client {clientId} is already registered for trip {tripId}.");

        var trip = await _tripRepository.GetByIdAsync(tripId)!;
        var currentCount = (await _clientTripRepository.GetAllByClientIdAsync(clientId)).Count();
        if (currentCount >= trip.MaxPeople)
            throw new InvalidOperationException("Trip capacity exceeded.");

        var registration = new ClientTrip
        {
            ClientId = clientId,
            TripId = tripId,
            RegisteredAt = DateTime.UtcNow
        };
        
        await _clientTripRepository.AddAsync(registration);
    }

    public async Task RemoveClientFromTripAsync(int clientId, int tripId)
    {
        if (!await _clientTripRepository.ExistsAsync(clientId, tripId))
            throw new InvalidOperationException($"Registration of client {clientId} for trip {tripId} not found.");

        await _clientTripRepository.RemoveAsync(clientId, tripId);
    }
}

