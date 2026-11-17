namespace CityBike;

public record Trip
{
    public string RideId { get; set; }
    public string RideableType { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set;}
    public string MemberCasual { get; set;}
    public string? StartStationId { get; set; }
    public string? EndStationId { get; set; }
    public double TotalMinutes => EndedAt.Subtract(StartedAt).TotalMinutes;
    public bool IsWeekend => StartedAt.DayOfWeek == DayOfWeek.Saturday || StartedAt.DayOfWeek == DayOfWeek.Sunday;
}