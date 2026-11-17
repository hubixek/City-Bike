using CsvHelper.Configuration.Attributes;
namespace CityBike;

public record CsvTrip
{
    [Name("ride_id")] public required string RideId { get; set; }
    [Name("rideable_type")] public required string RideableType { get; set; }
    [Name("started_at")] public DateTime StartedAt { get; set; }
    [Name("ended_at")] public DateTime EndedAt { get; set; }
    [Name("start_station_name")] public string? StartStationName { get; set; }
    [Name("start_station_id")] public string? StartStationId { get; set; }
    [Name("end_station_name")] public string? EndStationName { get; set; }
    [Name("end_station_id")] public string? EndStationId { get; set; }
    [Name("start_lat")] public double? StartLat { get; set; }
    [Name("start_lng")] public double? StartLng { get; set; }
    [Name("end_lat")] public double? EndLat { get; set; }
    [Name("end_lng")] public double? EndLng { get; set; }
    [Name("member_casual")] public required string MemberCasual { get; set; }
}