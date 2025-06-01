namespace Task9.Application.Services.Interfaces;

public interface IClientService
{
    Task<bool> DeleteClientAsync(int idClient);
}