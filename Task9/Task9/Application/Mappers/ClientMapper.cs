using Task9.Application.DTOs;
using Task9.Core.Models;

namespace Task9.Application.Mappers;

public static class ClientMapper
{
    public static ClientDto MapToCountryDto(this Client client)
    {
        return new ClientDto
        {
            FirstName = client.FirstName,
            LastName = client.LastName
        };
    }
}