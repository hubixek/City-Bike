# CityBike – analiza danych przejazdów w Nowym Jorku 

Aplikacja konsolowa w C#, która wczytuje rzeczywiste dane o przejazdach Citi Bike z Nowego Jorku, łączy je z historyczną pogodą i odpowiada na pytania przejazdów i pogody w październiku 2026 za pomocą zapytań LINQ. Wyniki są zapisywane do pliku `wyniki.md`.

Autor: Hubert Sukiennik

## Dane

| Dane | Źródło |
|---|---|
| Przejazdy Citi Bike (październik 2026) | [citibikenyc.com/system-data](https://citibikenyc.com/system-data), pliki do pobrania: [s3.amazonaws.com/tripdata](https://s3.amazonaws.com/tripdata/index.html) |
| Pogoda dzienna dla Nowego Jorku | [Open-Meteo Historical Weather API](https://open-meteo.com/en/docs/historical-weather-api) |

### Przejazdy
1. Pobierz z archiwum pliki `202610-citibike-tripdata`.
2. Rozpakuj je.
3. Wszystkie pliki CSV umieść w jednym, osobnym folderze poza repozytorium.

### Pogoda
1. Pobierz plik CSV z dziennymi danymi dla października 2026:

   `https://archive-api.open-meteo.com/v1/archive?latitude=40.71&longitude=-74.01&start_date=2026-10-01&end_date=2026-10-31&daily=temperature_2m_mean,precipitation_sum&timezone=America/New_York&format=csv`

2. Usuń 3 górne linijki z metadanymi.
3. Zamień nagłówek na `date,temp,rain`.
4. Zapisz plik poza folderem z przejazdami.

## Wymagania

Wymagany jest .NET SDK 10 oraz pakiet NuGet CsvHelper.

Program przyjmuje dwa argumenty: ścieżkę do **folderu** z plikami CSV z przejazdami oraz ścieżkę do pliku z pogodą:

```
dotnet run --project CityBike -- <folder_z_przejazdami> <plik_pogody.csv>
```

Wyniki trafiają do pliku `wyniki.md` w katalogu, z którego uruchomiono program.

## Jak to działa

1. Program czyta pliki CSV z przejazdami po jednym wierszu.
2. Błędne przejazdy są odrzucane.
3. Na wczytanych danych działa 6 zapytań LINQ, a wyniki trafiają do pliku `wyniki.md`.

Na pełnych danych: 5 087 442 przejazdów, 923 odrzucone, 2443 stacje.

## Zapytania LINQ

1. **Najbardziej oblegane stacje**: każdy przejazd rozbity na dwa wypożyczenie i zwrot, liczone w każdej parze stacja–godzina, zwraca top 5 stacji z ich najepszą godziną.
2. **Najpopularniejsze trasy**: top 10 par stacja startowa–stacja końcowa, z nazwami stacji.
3. **Bilans stacji**: stacje gdzie rowery się gromadzą i gdzie rowerów ubywa, z złączeniem wyników wyjazdów i przyjazdów.
4. **Wpływ deszczu**: średnia liczba przejazdów na dzień w dni suche i deszczowe, z złączeniem z pogodą po dacie.
5. **Udział rowerów elektrycznych w ciągu doby**: procent przejazdów elektrycznych w każdej godzinie.
6. **Wpływ temperatury**: średnia liczba przejazdów na dzień w przedziałach temperatury.
