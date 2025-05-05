using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task7.Models;
using Task7.Repositories.Interfaces;


namespace Task7.Repositories.Implementations;

public class CountryTripRepository : ICountryTripRepository
{
    private readonly string _connectionString;

    public CountryTripRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MyAppDb")!;
    }

    public async Task<IEnumerable<CountryTrip>> GetAllByTripIdAsync(int tripId)
    {
        var countryTrips = new List<CountryTrip>();
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        const string sql = @"SELECT IdCountry, IdTrip
                                 FROM dbo.Country_Trip
                                 WHERE IdTrip = @IdTrip;";
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@IdTrip", SqlDbType.Int) { Value = tripId });

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            countryTrips.Add(new CountryTrip
            {
                CountryId = reader.GetInt32(reader.GetOrdinal("IdCountry")),
                TripId    = reader.GetInt32(reader.GetOrdinal("IdTrip"))
            });
        }

        return countryTrips;
    }
}


