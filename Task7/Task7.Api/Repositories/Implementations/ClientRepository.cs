using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task7.Models;
using Task7.Repositories.Interfaces;

namespace Task7.Repositories.Implementations
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MyAppDb");
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            var clients = new List<Client>();
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"SELECT Id, Pesel, FirstName, LastName, BirthDate, Email FROM dbo.Client;";
            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                clients.Add(new Client
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Pesel = reader.GetString(reader.GetOrdinal("Pesel")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                    Email = reader.GetString(reader.GetOrdinal("Email"))
                });
            }

            return clients;
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"SELECT Id, Pesel, FirstName, LastName, BirthDate, Email
                                FROM dbo.Client
                                WHERE Id = @Id;";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Client
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Pesel = reader.GetString(reader.GetOrdinal("Pesel")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                    Email = reader.GetString(reader.GetOrdinal("Email"))
                };
            }

            return null;
        }

        public async Task<Client?> GetByPeselAsync(string pesel)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"SELECT Id, Pesel, FirstName, LastName, BirthDate, Email
                                FROM dbo.Client
                                WHERE Pesel = @Pesel;";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Pesel", SqlDbType.NVarChar, 11) { Value = pesel });

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Client
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Pesel = reader.GetString(reader.GetOrdinal("Pesel")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                    Email = reader.GetString(reader.GetOrdinal("Email"))
                };
            }

            return null;
        }

        public async Task AddAsync(Client client)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"INSERT INTO dbo.Client (Pesel, FirstName, LastName, BirthDate, Email)
                                VALUES (@Pesel, @FirstName, @LastName, @BirthDate, @Email);";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Pesel", SqlDbType.NVarChar, 11) { Value = client.Pesel });
            cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Value = client.FirstName });
            cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Value = client.LastName });
            cmd.Parameters.Add(new SqlParameter("@BirthDate", SqlDbType.DateTime) { Value = client.BirthDate });
            cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = client.Email });

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"SELECT COUNT(1) FROM dbo.Client WHERE Id = @Id;";
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            var count = (int)await cmd.ExecuteScalarAsync();
            return count > 0;
        }
    }
}
