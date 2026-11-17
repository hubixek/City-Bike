using CityBike;

var folder = args[0];
var sw = System.Diagnostics.Stopwatch.StartNew();

var trips = TripLoader.LoadTrips(folder);
var stations = TripLoader.LoadStations(folder);

foreach (var s in Queries.Top5BusiestStation(trips, stations))
    Console.WriteLine($"{s.Name} | {s.Hour:dd.MM HH:mm} | {s.Count}");

Console.WriteLine();
Console.WriteLine("Top 10 tras:");
foreach (var r in Queries.Top10PopularRoutes(trips, stations))
    Console.WriteLine($"{r.From} → {r.To} | {r.Count}");