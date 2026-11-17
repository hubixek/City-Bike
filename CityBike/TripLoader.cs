using System.Globalization;
using CsvHelper;

namespace CityBike;

public static class TripLoader
{
    public static IEnumerable<CsvTrip> ReadFile(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        foreach (var trip in csv.GetRecords<CsvTrip>())
        {
            yield return trip;
        }
    }

    public static IEnumerable<CsvTrip> ReadFolder(string path) =>
        Directory.EnumerateFiles(path, "*.csv").SelectMany(ReadFile);

    public static Trip ToTrip(CsvTrip csvTrip)
    {
        Trip trip = new Trip();
        trip.RideId = csvTrip.RideId;
        trip.RideableType = csvTrip.RideableType;
        trip.MemberCasual = csvTrip.MemberCasual;
        trip.EndedAt = csvTrip.EndedAt;
        trip.StartedAt = csvTrip.StartedAt;
        trip.StartStationId = csvTrip.StartStationId;
        trip.EndStationId = csvTrip.EndStationId;
        return trip;
    }

    public static bool IsValid(CsvTrip csvTrip)
    {
        if (csvTrip.EndedAt > csvTrip.StartedAt && csvTrip.EndedAt.Subtract(csvTrip.StartedAt).TotalHours < 24)
        {
            return true;
        }
        return  false;
    }

    public static List<Trip> LoadTrips(string folder) =>
         TripLoader.ReadFolder(folder)
            .Where(TripLoader.IsValid)
            .Select(TripLoader.ToTrip)
            .ToList();

    public static Dictionary<string, Station> LoadStations(string folder)
    {
        var s = TripLoader.ReadFolder(folder).SelectMany(t => new[]
        {
            (Id: t.StartStationId, Name: t.StartStationName, Lat: t.StartLat, Lng: t.StartLng),
            (Id: t.EndStationId,   Name: t.EndStationName,   Lat: t.EndLat,   Lng: t.EndLng)
        })
        .Where(t => t.Id != null && t.Name != null && t.Lat != null && t.Lng != null)
        .DistinctBy(t => t.Id)
        .Select(t => new Station { Id = t.Id, Name = t.Name, Latitude = t.Lat.Value, Longitude = t.Lng.Value })
        .ToDictionary(T => T.Id);
        return s;
    }
}