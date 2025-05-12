using Microsoft.Data.SqlClient;

namespace Warehouse.API.Repositories;

public interface IWarehouseRepository
{
    // non-transactional existence checks
    Task<bool> ProductExistsAsync(int productId);
    Task<bool> WarehouseExistsAsync(int warehouseId);

    // transactional ones—pass in an open conn+tx
    Task<int?> FindUnfulfilledOrderAsync(int productId, int amount, DateTime createdAt,
        SqlConnection conn, SqlTransaction tx);
    Task MarkOrderFulfilledAsync(int orderId,
        SqlConnection conn, SqlTransaction tx);
    Task<int> InsertProductWarehouseAsync(int orderId, int warehouseId, int productId, int amount,
        SqlConnection conn, SqlTransaction tx);
}