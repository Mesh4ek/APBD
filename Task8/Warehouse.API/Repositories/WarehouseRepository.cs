using Microsoft.Data.SqlClient;

namespace Warehouse.API.Repositories;


public class WarehouseRepository : IWarehouseRepository
{
    
    private readonly string? _connectionString;

    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }
    
    public async Task<bool> ProductExistsAsync(int productId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(
            "SELECT COUNT(1) FROM Product WHERE IdProduct = @id", connection);
        command.Parameters.AddWithValue("@id", productId);
        
        var result = await command.ExecuteScalarAsync();      
        var count  = Convert.ToInt32(result);                
        return count > 0;
    }

    public async Task<bool> WarehouseExistsAsync(int warehouseId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand(
            "SELECT COUNT(1) FROM Warehouse WHERE IdWarehouse = @id", connection);
        command.Parameters.AddWithValue("@id", warehouseId);
        
        var result = await command.ExecuteScalarAsync();      
        var count  = Convert.ToInt32(result);            
        return count > 0;
    }

    public async Task<int?> FindUnfulfilledOrderAsync(int productId, int amount, DateTime createdAt, SqlConnection conn, SqlTransaction tx)
    {
       using var command = new SqlCommand(@"
                SELECT TOP(1) IdOrder
                  FROM [Order]
                 WHERE IdProduct = @pid
                   AND Amount = @amt
                   AND CreatedAt < @createdAt
                   AND IdOrder NOT IN (SELECT IdOrder FROM Product_Warehouse)", conn, tx);
       command.Parameters.AddWithValue("@pid", productId);
       command.Parameters.AddWithValue("@amt", amount);
       command.Parameters.AddWithValue("@createdAt", createdAt);
        
        var result = await command.ExecuteScalarAsync();
        return result == null || result is DBNull
            ? (int?)null
            : Convert.ToInt32(result);
    }

    public async Task MarkOrderFulfilledAsync(int orderId, SqlConnection conn, SqlTransaction tx)
    {
        using var command = new SqlCommand("UPDATE [Order] SET FulfilledAt = GETUTCDATE() WHERE IdOrder = @oid", conn, tx);
        command.Parameters.AddWithValue("@oid", orderId);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<int> InsertProductWarehouseAsync(int orderId, int warehouseId, int productId, int amount, SqlConnection conn, SqlTransaction tx)
    {
        using var command = new SqlCommand(@"
                    DECLARE @unitPrice DECIMAL(18,2);
                    SELECT @unitPrice = Price FROM Product WHERE IdProduct = @pid;
                    
                    INSERT INTO Product_Warehouse
                                (IdOrder, IdWarehouse, IdProduct, Amount, Price, CreatedAt)
                    VALUES (@id, @wid, @pid, @amt, @unitPrice * @amt, GETUTCDATE()); 
                    
                    SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tx);
        command.Parameters.AddWithValue("@id", orderId);
        command.Parameters.AddWithValue("@wid", warehouseId);
        command.Parameters.AddWithValue("@pid", productId);
        command.Parameters.AddWithValue("@amt", amount);
        
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }
}