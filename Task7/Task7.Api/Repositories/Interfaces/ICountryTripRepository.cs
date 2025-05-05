using Task7.Models;

namespace Task7.Repositories.Interfaces;

public interface ICountryTripRepository
{
    Task<IEnumerable<CountryTrip>> GetAllByTripIdAsync(int tripId);
}