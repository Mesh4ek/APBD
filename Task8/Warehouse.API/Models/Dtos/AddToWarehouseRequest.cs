using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Models.Dtos;

public class AddToWarehouseRequest
{
    [Required]
    public int ProductId    { get; set; }
    [Required]
    public int WarehouseId  { get; set; }
    [Required]
    public int Amount       { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}