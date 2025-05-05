using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task7.Models;
using Task7.Repositories.Interfaces;

namespace Task7.Repositories.Implementations;
public class ClientTripRepository : IClientTripRepository
    {
        private readonly string _connectionString;

        public ClientTripRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyAppDb")!;
        }

        public async Task<IEnumerable<ClientTrip>> GetAllByClientIdAsync(int clientId)
        {
            var registrations = new List<ClientTrip>();
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"SELECT IdClient, IdTrip, RegisteredAt
                                 FROM dbo.Client_Trip
                                 WHERE IdClient = @IdClient;";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@IdClient", SqlDbType.Int) { Value = clientId });

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                registrations.Add(new ClientTrip
                {
                    ClientId     = reader.GetInt32(reader.GetOrdinal("IdClient")),
                    TripId       = reader.GetInt32(reader.GetOrdinal("IdTrip")),
                    RegisteredAt = reader.GetDateTime(reader.GetOrdinal("RegisteredAt"))
                });
            }

            return registrations;
        }

        public async Task<bool> ExistsAsync(int clientId, int tripId)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"SELECT COUNT(1)
                                 FROM dbo.Client_Trip
                                 WHERE IdClient = @IdClient AND IdTrip = @IdTrip;";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@IdClient", SqlDbType.Int) { Value = clientId });
            cmd.Parameters.Add(new SqlParameter("@IdTrip",   SqlDbType.Int) { Value = tripId });

            var count = (int) await cmd.ExecuteScalarAsync();
            return count > 0;
        }

        public async Task AddAsync(ClientTrip clientTrip)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"INSERT INTO dbo.Client_Trip (IdClient, IdTrip, RegisteredAt)
                                 VALUES (@IdClient, @IdTrip, @RegisteredAt);";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@IdClient",     SqlDbType.Int)      { Value = clientTrip.ClientId });
            cmd.Parameters.Add(new SqlParameter("@IdTrip",       SqlDbType.Int)      { Value = clientTrip.TripId });
            cmd.Parameters.Add(new SqlParameter("@RegisteredAt", SqlDbType.DateTime) { Value = clientTrip.RegisteredAt });

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RemoveAsync(int clientId, int tripId)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"DELETE FROM dbo.Client_Trip
                                 WHERE IdClient = @IdClient AND IdTrip = @IdTrip;";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@IdClient", SqlDbType.Int) { Value = clientId });
            cmd.Parameters.Add(new SqlParameter("@IdTrip",   SqlDbType.Int) { Value = tripId });

            await cmd.ExecuteNonQueryAsync();
        }
    }
