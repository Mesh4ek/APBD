using Warehouse.API.Models.Dtos;

namespace Warehouse.API.Services;

public interface IWarehouseService
{
    Task<int> AddToWarehouseAsync(AddToWarehouseRequest req);
}