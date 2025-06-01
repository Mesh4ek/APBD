using Task9.Application.DTOs;
using Task9.Core.Models;

namespace Task9.Application.Mappers;

public static class CountryMapper
{
    public static CountryDto MapToCountryDto(this Country country)
    {
        return new CountryDto
        {
            Name = country.Name
        };
    }
}