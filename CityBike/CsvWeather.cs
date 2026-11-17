using CsvHelper.Configuration.Attributes;
namespace CityBike;

public record CsvWeather
{
    [Name("date")] public DateTime Date { get; set; }
    [Name("temp")] public double Temp { get; set; }
    [Name("rain")] public double Rain { get; set; }
}