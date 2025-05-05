namespace Task7.Contracts.Responses;

public class GetAllTripsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxPeople { get; set; }
    public decimal Price { get; set; }
    public List<CountryResponse> Countries { get; set; } = new();
}