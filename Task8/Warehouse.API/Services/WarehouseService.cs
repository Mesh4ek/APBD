using Microsoft.Data.SqlClient;
using Warehouse.API.Models.Dtos;
using Warehouse.API.Repositories;

namespace Warehouse.API.Services;

public class WarehouseService : IWarehouseService
{
    
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly string? _connectionString;

    public WarehouseService(IWarehouseRepository warehouseRepository, IConfiguration configuration)
    {
        _warehouseRepository = warehouseRepository;
        _connectionString = configuration.GetConnectionString("Default");
    }


    public async Task<int> AddToWarehouseAsync(AddToWarehouseRequest req)
    {
        if (req.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");
        
        if (!await _warehouseRepository.ProductExistsAsync(req.ProductId))
            throw new KeyNotFoundException($"Product {req.ProductId} not found.");
        if (!await _warehouseRepository.WarehouseExistsAsync(req.WarehouseId))
            throw new KeyNotFoundException($"Warehouse {req.WarehouseId} not found.");

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = connection.BeginTransaction();

        try
        {
            var orderId = await _warehouseRepository.FindUnfulfilledOrderAsync(
                req.ProductId, req.Amount, req.CreatedAt, connection, transaction);
            if (orderId == null)
                throw new InvalidOperationException($"Order {req.ProductId} not found.");
            
            await _warehouseRepository.MarkOrderFulfilledAsync(orderId.Value, connection, transaction);
            
            var newId = await _warehouseRepository.InsertProductWarehouseAsync(
                orderId.Value, req.WarehouseId, req.ProductId,req.Amount, connection, transaction);
            
            await transaction.CommitAsync();
            return newId;
        }
        catch 
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}