namespace CityBike;

public static class Queries
{
   public static List<(string Name, DateTime Hour, int Count)> Top5BusiestStation (List<Trip> trips, Dictionary<string,Station> stations)
   {
      var res = trips.SelectMany(t => new[]
         {
            (StationId: t.StartStationId, Time: t.StartedAt),
            (StationId: t.EndStationId, Time: t.EndedAt)
         })
         .Where(s => s.StationId != null)
         .Select(s => (s.StationId, Hour: s.Time.Date.AddHours(s.Time.Hour)))
         .GroupBy(s => (s.StationId, s.Hour))
         .Select(s => (s.Key.StationId, s.Key.Hour, Count: s.Count()))
         .GroupBy(s => s.StationId)
         .Select(g => g.MaxBy(x => x.Count))
         .OrderByDescending(s => s.Count)
         .Take(5)
         .Select(g => (Name: stations[g.StationId].Name, g.Hour, g.Count));
      return res.ToList();
   }

   public static List<(string From, string To, int Count)> Top10PopularRoutes(List<Trip> trips,
      Dictionary<string, Station> stations)
   {
      var res = trips.Where(t => t.StartStationId != null && t.EndStationId != null)
         .GroupBy(t => (t.StartStationId, t.EndStationId))
         .Select(t => (FromId: t.Key.StartStationId, ToId: t.Key.EndStationId, Count: t.Count()))
         .OrderByDescending(t => t.Count).Take(10)
         .Join(stations.Values, t => t.FromId, s => s.Id,
            (t, s) => (From: s.Name, t.ToId, t.Count))
         .Join(stations.Values, t => t.ToId, s => s.Id,
            (t, s) => (t.From, To: s.Name, t.Count))
         .ToList();
      return res;
   }

   public static List<(string Name, int Out, int In, int Net)> StationBalance(List<Trip> trips,
         Dictionary<string, Station> stations)
      {
         var dep = trips.Where(t => t.StartStationId != null)
            .GroupBy(t => t.StartStationId)
            .Select(t => (Id: t.Key, Out: t.Count()));
         var arr = trips.Where(t => t.EndStationId != null)
            .GroupBy(t => t.EndStationId)
            .Select(t => (Id: t.Key, In: t.Count()));
         var res = dep.Join(arr, d => d.Id, a => a.Id, (d, a) => (d.Id, d.Out, a.In, Net: a.In - d.Out))
            .OrderByDescending(t => t.Net)
            .Select(t => (stations[t.Id!].Name, t.Out, t.In, t.Net))
            .ToList();
         return res;
      }
   public static List<(string Category, int Days, double AvgRides)> WeatherImpact(
      List<Trip> trips, List<CsvWeather> weather)
   {
      return trips
         .GroupBy(t => t.StartedAt.Date)
         .Select(t => (Date: t.Key, Count: t.Count()))
         .Join(weather, d => d.Date, w => w.Date, (d, w) => (d.Count, w.Rain))
         .GroupBy(t => t.Rain > 1 ? "Deszczowe" : "Suche")
         .Select(t => (Category: t.Key, Days: t.Count(), AvgRides: t.Average(t => t.Count)))
         .ToList();
   }
   public static List<(int Hour, int Total, double ElectricPercent)> TypeShareByHour(List<Trip> trips)
   {
      return trips
         .GroupBy(t => t.StartedAt.Hour)
         .Select(t => (Hour: t.Key, Total: t.Count(),
            ElectricPercent: 100.0 * t.Count(t => t.RideableType == "electric_bike") / t.Count()))
         .OrderBy(t => t.Hour)
         .ToList();
   }
}