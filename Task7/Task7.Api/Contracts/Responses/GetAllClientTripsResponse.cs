namespace Task7.Contracts.Responses;

public class GetAllClientTripsResponse
{
    public int TripId { get; set; }
    public string TripName { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public List<CountryResponse>? Countries { get; set; }
}