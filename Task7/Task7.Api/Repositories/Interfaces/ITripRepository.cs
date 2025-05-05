using Task7.Models;

namespace Task7.Repositories.Interfaces;

public interface ITripRepository
{
    Task<IEnumerable<Trip>> GetAllAsync();
    Task<Trip?> GetByIdAsync(int id);
    Task AddAsync(Trip trip);
    Task<bool> ExistsAsync(int id);
}