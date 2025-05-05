using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task7.Models;
using Task7.Repositories.Interfaces;

namespace Task7.Repositories.Implementations
{
    public class TripRepository : ITripRepository
    {
        private readonly string _connectionString;

        public TripRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyAppDb")!;
        }

        public async Task<IEnumerable<Trip>> GetAllAsync()
        {
            var trips = new List<Trip>();
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            // Select Id (not TripId) and map it to Trip.Id
            const string sql = @"
                SELECT Id, Name, StartDate, EndDate, MaxPeople, Price
                FROM dbo.Trip;
            ";
            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                trips.Add(new Trip
                {
                    Id        = reader.GetInt32  (reader.GetOrdinal("Id")),
                    Name      = reader.GetString (reader.GetOrdinal("Name")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    EndDate   = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                    MaxPeople = reader.GetInt32  (reader.GetOrdinal("MaxPeople")),
                    Price     = reader.GetDecimal(reader.GetOrdinal("Price"))
                });
            }

            return trips;
        }

        public async Task<Trip?> GetByIdAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                SELECT Id, Name, StartDate, EndDate, MaxPeople, Price
                FROM dbo.Trip
                WHERE Id = @Id;
            ";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new Trip
            {
                Id        = reader.GetInt32  (reader.GetOrdinal("Id")),
                Name      = reader.GetString (reader.GetOrdinal("Name")),
                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate   = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                MaxPeople = reader.GetInt32  (reader.GetOrdinal("MaxPeople")),
                Price     = reader.GetDecimal(reader.GetOrdinal("Price"))
            };
        }

        public async Task AddAsync(Trip trip)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                INSERT INTO dbo.Trip (Name, StartDate, EndDate, MaxPeople, Price)
                VALUES (@Name, @StartDate, @EndDate, @MaxPeople, @Price);
            ";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Name",      SqlDbType.NVarChar, 100) { Value = trip.Name });
            cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)       { Value = trip.StartDate });
            cmd.Parameters.Add(new SqlParameter("@EndDate",   SqlDbType.DateTime)       { Value = trip.EndDate });
            cmd.Parameters.Add(new SqlParameter("@MaxPeople", SqlDbType.Int)            { Value = trip.MaxPeople });
            cmd.Parameters.Add(new SqlParameter("@Price",     SqlDbType.Decimal)        { Value = trip.Price });

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.Trip
                WHERE Id = @Id;
            ";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
    }
}
