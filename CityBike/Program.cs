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

var balance = Queries.StationBalance(trips, stations);

Console.WriteLine();
Console.WriteLine("Ujścia (rowery się gromadzą):");
foreach (var s in balance.Take(5))
    Console.WriteLine($"{s.Name} | out {s.Out} | in {s.In} | {s.Net}");

Console.WriteLine();
Console.WriteLine("Źródła (rowerów ubywa):");
foreach (var s in balance.TakeLast(5))
    Console.WriteLine($"{s.Name} | out {s.Out} | in {s.In} | {s.Net}");