using System.ComponentModel.DataAnnotations;

namespace Task5.Api.Contracts.Requests;

public class CreateAnimalRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Category { get; set; } = string.Empty;
    [Required] public double Weight { get; set; }
    [Required] public string FurColor { get; set; } = string.Empty;
}