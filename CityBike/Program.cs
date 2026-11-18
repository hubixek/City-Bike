using CityBike;

var folder = args[0];
var weatherPath = args[1];
var sw = System.Diagnostics.Stopwatch.StartNew();

// Wczytywanie
var trips = TripLoader.LoadTrips(folder);
var stations = TripLoader.LoadStations(folder);
var weather = TripLoader.LoadWeather(weatherPath);
var loadTime = sw.Elapsed.TotalSeconds;

using var writer = new StreamWriter("wyniki.md");

writer.WriteLine("# CityBike – wyniki analizy - październik 2026");
writer.WriteLine();
writer.WriteLine($"- Przejazdy po walidacji: {trips.Count}");
writer.WriteLine($"- Stacje: {stations.Count}");
writer.WriteLine($"- Czas wczytywania: {loadTime:F1} s");
writer.WriteLine();

// Najbardziej oblegane stacje
writer.WriteLine("## Top 5 najbardziej obleganych stacji (wypożyczenia + zwroty w ciągu godziny)");
writer.WriteLine();
writer.WriteLine("| Stacja | Godzina szczytu | Liczba rowerów |");
writer.WriteLine("|---|---|---|");
foreach (var s in Queries.Top5BusiestStation(trips, stations))
    writer.WriteLine($"| {s.Name} | {s.Hour:dd.MM HH:mm} | {s.Count} |");
writer.WriteLine();

// Najpopularniejsze trasy
writer.WriteLine("## Top 10 najpopularniejszych tras");
writer.WriteLine();
writer.WriteLine("| Skąd | Dokąd | Przejazdy |");
writer.WriteLine("|---|---|---|");
foreach (var r in Queries.Top10PopularRoutes(trips, stations))
    writer.WriteLine($"| {r.From} | {r.To} | {r.Count} |");
writer.WriteLine();

// Bilans stacji
var balance = Queries.StationBalance(trips, stations);

writer.WriteLine("## Stacje gdzie rowery się gromadzą");
writer.WriteLine();
writer.WriteLine("| Stacja | Wyjazdy | Przyjazdy | Bilans |");
writer.WriteLine("|---|---|---|---|");
foreach (var s in balance.Take(5))
    writer.WriteLine($"| {s.Name} | {s.Out} | {s.In} | {s.Net} |");
writer.WriteLine();

writer.WriteLine("## Stacje gdzie rowerów ubywa");
writer.WriteLine();
writer.WriteLine("| Stacja | Wyjazdy | Przyjazdy | Bilans |");
writer.WriteLine("|---|---|---|---|");
foreach (var s in balance.TakeLast(5))
    writer.WriteLine($"| {s.Name} | {s.Out} | {s.In} | {s.Net} |");
writer.WriteLine();

// Pogoda
writer.WriteLine("## Wpływ pogody na liczbę przejazdów");
writer.WriteLine();
writer.WriteLine("| Dni | Liczba dni | Średnio przejazdów |");
writer.WriteLine("|---|---|---|");
foreach (var w in Queries.WeatherImpact(trips, weather))
    writer.WriteLine($"| {w.Category} | {w.Days} | {w.AvgRides:F0} |");
writer.WriteLine();

// Udział rowerów elektrycznych w ciągu doby
writer.WriteLine("## Udział rowerów elektrycznych w ciągu doby");
writer.WriteLine();
writer.WriteLine("| Godzina | Przejazdy | Elektryczne |");
writer.WriteLine("|---|---|---|");
foreach (var h in Queries.TypeShareByHour(trips))
    writer.WriteLine($"| {h.Hour:00}:00 | {h.Total} | {h.ElectricPercent:F1}% |");

// Wpływ temperatury na liczbę przejazdów
writer.WriteLine("## Wpływ temperatury na liczbę przejazdów");
writer.WriteLine();
writer.WriteLine("| Temperatura | Liczba dni | Średnia przejazdów |");
writer.WriteLine("|---|---|---|");
foreach (var h in Queries.TemperatureImpact(trips, weather))
    writer.WriteLine($"| {h.temp} - {h.temp+3}°C | {h.Count} | {h.AvgRides:F0} |");
writer.WriteLine();
Console.WriteLine($"Gotowe w {sw.Elapsed.TotalSeconds:F1} s. Wyniki zapisano do wyniki.md");