using System.Collections.Generic;
using System.Data;
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
            _connectionString = configuration.GetConnectionString("MyAppDb");
        }

        public async Task<IEnumerable<Trip>> GetAllAsync()
        {
            var trips = new List<Trip>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"SELECT Id, Name, StartDate, EndDate, MaxPeople, Price FROM dbo.Trip;";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                trips.Add(new Trip
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    StartDate = reader.GetDateTime("StartDate"),
                    EndDate = reader.GetDateTime("EndDate"),
                    MaxPeople = reader.GetInt32("MaxPeople"),
                    Price = reader.GetDecimal("Price")
                });
            }

            return trips;
        }

        public async Task<Trip?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"SELECT Id, Name, StartDate, EndDate, MaxPeople, Price FROM dbo.Trip WHERE Id = @Id;";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Trip
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    StartDate = reader.GetDateTime("StartDate"),
                    EndDate = reader.GetDateTime("EndDate"),
                    MaxPeople = reader.GetInt32("MaxPeople"),
                    Price = reader.GetDecimal("Price")
                };
            }

            return null;
        }

        public async Task AddAsync(Trip trip)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"INSERT INTO dbo.Trip (Name, StartDate, EndDate, MaxPeople, Price)
                        VALUES (@Name, @StartDate, @EndDate, @MaxPeople, @Price);";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", trip.Name);
            cmd.Parameters.AddWithValue("@StartDate", trip.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", trip.EndDate);
            cmd.Parameters.AddWithValue("@MaxPeople", trip.MaxPeople);
            cmd.Parameters.AddWithValue("@Price", trip.Price);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "SELECT COUNT(1) FROM dbo.Trip WHERE Id = @Id;";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            var count = (int)await cmd.ExecuteScalarAsync();
            return count > 0;
        }
    }
}
