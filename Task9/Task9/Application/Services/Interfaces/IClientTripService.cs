using Task9.Application.DTOs;

namespace Task9.Application.Services.Interfaces;

public interface IClientTripService
{
    Task RegisterClientToTripAsync(int idTrip, RegisterClientDto dto);
}