using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task7.Contracts.Requests;
using Task7.Contracts.Responses;
using Task7.Models;
using Task7.Repositories.Interfaces;
using Task7.Services.Interfaces;

namespace Task7.Services.Implementations;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly ICountryTripRepository _countryTripRepository;

    public TripService(
        ITripRepository tripRepository,
        ICountryTripRepository countryTripRepository)
    {
        _tripRepository = tripRepository;
        _countryTripRepository = countryTripRepository;
    }

    public async Task<IEnumerable<GetAllTripsResponse>> GetAllTripsAsync()
    {
        var trips = await _tripRepository.GetAllAsync();
        var responses = new List<GetAllTripsResponse>();

        foreach (var trip in trips)
        {
            var links = await _countryTripRepository.GetAllByTripIdAsync(trip.Id);
            var countries = links
                .Select(ct => new CountryResponse { Id = ct.CountryId, Name = string.Empty })
                .ToList();

            responses.Add(new GetAllTripsResponse
            {
                Id = trip.Id,
                Name = trip.Name,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                MaxPeople = trip.MaxPeople,
                Price = trip.Price,
                Countries = countries
            });
        }

        return responses;
    }

    public async Task<GetAllTripsResponse?> GetTripByIdAsync(int id)
    {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null)
            throw new InvalidOperationException($"Trip with ID {id} does not exist.");

        var links = await _countryTripRepository.GetAllByTripIdAsync(id);
        var countries = links
            .Select(ct => new CountryResponse { Id = ct.CountryId, Name = string.Empty })
            .ToList();

        return new GetAllTripsResponse
        {
            Id = trip.Id,
            Name = trip.Name,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate,
            MaxPeople = trip.MaxPeople,
            Price = trip.Price,
            Countries = countries
        };
    }
}