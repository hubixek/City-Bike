using CityBike;

var folder = args[0];
var sw = System.Diagnostics.Stopwatch.StartNew();

var trips = TripLoader.LoadTrips(folder);
var stations = TripLoader.LoadStations(folder);

// foreach (var s in Queries.Top5BusiestStation(trips, stations))
//     Console.WriteLine($"{s.Name} | {s.Hour:dd.MM HH:mm} | {s.Count}");
//
// Console.WriteLine();
// Console.WriteLine("Top 10 tras:");
// foreach (var r in Queries.Top10PopularRoutes(trips, stations))
//     Console.WriteLine($"{r.From} → {r.To} | {r.Count}");
//
// var balance = Queries.StationBalance(trips, stations);
//
// Console.WriteLine();
// Console.WriteLine("Ujścia (rowery się gromadzą):");
// foreach (var s in balance.Take(5))
//     Console.WriteLine($"{s.Name} | out {s.Out} | in {s.In} | {s.Net}");
//
// Console.WriteLine();
// Console.WriteLine("Źródła (rowerów ubywa):");
// foreach (var s in balance.TakeLast(5))
//     Console.WriteLine($"{s.Name} | out {s.Out} | in {s.In} | {s.Net}");
// var weather = TripLoader.LoadWeather(args[1]);
// Console.WriteLine();
// Console.WriteLine("Wpływ pogody:");
// foreach (var w in Queries.WeatherImpact(trips, weather))
//     Console.WriteLine($"{w.Category} | {w.Days} dni | średnio {w.AvgRides:F0} przejazdów/dzień");
//     
Console.WriteLine();
Console.WriteLine("Udział rowerów elektrycznych w ciągu doby:");
foreach (var h in Queries.TypeShareByHour(trips))
    Console.WriteLine($"{h.Hour:00}:00 | {h.Total} przejazdów | {h.ElectricPercent:F1}% elektrycznych");