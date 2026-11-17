using CityBike;

var folder = args[0];
var sw = System.Diagnostics.Stopwatch.StartNew();

var trips = TripLoader.LoadTrips(folder);
var stations = TripLoader.LoadStations(folder);

Console.WriteLine($"Przejazdy po walidacji: {trips.Count}");
Console.WriteLine($"Odrzucone: {5088365 - trips.Count}");
Console.WriteLine($"Stacje: {stations.Count}");
Console.WriteLine($"Czas wczytywania: {sw.Elapsed.TotalSeconds:F1} s");