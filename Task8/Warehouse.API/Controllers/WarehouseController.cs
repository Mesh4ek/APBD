using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Models.Dtos;
using Warehouse.API.Services;

namespace Warehouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : Controller
{
    private readonly IWarehouseService _warehouseService;
    
    public WarehouseController(IWarehouseService warehouseService) => _warehouseService = warehouseService;

    [HttpPost("add")]
    public async Task<ActionResult<int>> AddToWarehouse([FromBody] AddToWarehouseRequest req)
    {
        try
        {
            var newId = await _warehouseService.AddToWarehouseAsync(req);
            return Ok(newId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}