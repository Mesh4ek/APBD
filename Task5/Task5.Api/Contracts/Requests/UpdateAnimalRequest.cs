using System.ComponentModel.DataAnnotations;

namespace Task5.Api.Contracts.Requests;

public class UpdateAnimalRequest
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double Weight { get; set; }
    public string FurColor { get; set; } = string.Empty;
}