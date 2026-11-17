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
         .Select(t =>(FromId: t.Key.StartStationId, ToId: t.Key.EndStationId, Count: t.Count()))
         .OrderByDescending(t => t.Count).Take(10)
         .Join(stations.Values, t => t.FromId, s => s.Id,
            (t, s) => (From: s.Name, t.ToId, t.Count))
         .Join(stations.Values, t => t.ToId, s => s.Id,
               (t, s) => (t.From, To: s.Name, t.Count))
         .ToList();
      return res;
   }
}