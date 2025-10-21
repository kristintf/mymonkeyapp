using System.Text.Json;
using MyMonkeyApp.Models;

namespace MyMonkeyApp.Helpers;

/// <summary>
/// Static helper class for managing monkey species data from MonkeyMCP server
/// Includes access tracking and comprehensive monkey management functionality
/// </summary>
public static class MonkeyHelper
{
    private static readonly object _lock = new();
    private static List<Monkey>? _monkeys;
    private static readonly Dictionary<string, int> _accessCounts = new();
    private static int _totalAccesses = 0;
    private static DateTime _lastDataLoad = DateTime.MinValue;

    /// <summary>
    /// MonkeyMCP data (from the actual MCP server response)
    /// </summary>
    private static readonly string _monkeyMcpData = """
    [
        {
            "Name": "Baboon",
            "Location": "Africa & Asia",
            "Details": "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/baboon.jpg",
            "Population": 10000,
            "Latitude": -8.783195,
            "Longitude": 34.508523
        },
        {
            "Name": "Capuchin Monkey",
            "Location": "Central & South America",
            "Details": "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/capuchin.jpg",
            "Population": 23000,
            "Latitude": 12.769013,
            "Longitude": -85.602364
        },
        {
            "Name": "Blue Monkey",
            "Location": "Central and East Africa",
            "Details": "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/bluemonkey.jpg",
            "Population": 12000,
            "Latitude": 1.957709,
            "Longitude": 37.297204
        },
        {
            "Name": "Squirrel Monkey",
            "Location": "Central & South America",
            "Details": "The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/saimiri.jpg",
            "Population": 11000,
            "Latitude": -8.783195,
            "Longitude": -55.491477
        },
        {
            "Name": "Golden Lion Tamarin",
            "Location": "Brazil",
            "Details": "The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/tamarin.jpg",
            "Population": 19000,
            "Latitude": -14.235004,
            "Longitude": -51.92528
        },
        {
            "Name": "Howler Monkey",
            "Location": "South America",
            "Details": "Howler monkeys are among the largest of the New World monkeys. Fifteen species are currently recognised. Previously classified in the family Cebidae, they are now placed in the family Atelidae.",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/alouatta.jpg",
            "Population": 8000,
            "Latitude": -8.783195,
            "Longitude": -55.491477
        },
        {
            "Name": "Japanese Macaque",
            "Location": "Japan",
            "Details": "The Japanese macaque, is a terrestrial Old World monkey species native to Japan. They are also sometimes known as the snow monkey because they live in areas where snow covers the ground for months each",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/macasa.jpg",
            "Population": 1000,
            "Latitude": 36.204824,
            "Longitude": 138.252924
        },
        {
            "Name": "Mandrill",
            "Location": "Southern Cameroon, Gabon, and Congo",
            "Details": "The mandrill is a primate of the Old World monkey family, closely related to the baboons and even more closely to the drill. It is found in southern Cameroon, Gabon, Equatorial Guinea, and Congo.",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/mandrill.jpg",
            "Population": 17000,
            "Latitude": 7.369722,
            "Longitude": 12.354722
        },
        {
            "Name": "Proboscis Monkey",
            "Location": "Borneo",
            "Details": "The proboscis monkey or long-nosed monkey, known as the bekantan in Malay, is a reddish-brown arboreal Old World monkey that is endemic to the south-east Asian island of Borneo.",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/borneo.jpg",
            "Population": 15000,
            "Latitude": 0.961883,
            "Longitude": 114.55485
        },
        {
            "Name": "Sebastian",
            "Location": "Seattle",
            "Details": "This little trouble maker lives in Seattle with James and loves traveling on adventures with James and tweeting @MotzMonkeys. He by far is an Android fanboy and is getting ready for the new Google Pixel 9!",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/sebastian.jpg",
            "Population": 1,
            "Latitude": 47.606209,
            "Longitude": -122.332071
        },
        {
            "Name": "Henry",
            "Location": "Phoenix",
            "Details": "An adorable Monkey who is traveling the world with Heather and live tweets his adventures @MotzMonkeys. His favorite platform is iOS by far and is excited for the new iPhone Xs!",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/henry.jpg",
            "Population": 1,
            "Latitude": 33.448377,
            "Longitude": -112.074037
        },
        {
            "Name": "Red-shanked douc",
            "Location": "Vietnam",
            "Details": "The red-shanked douc is a species of Old World monkey, among the most colourful of all primates. The douc is an arboreal and diurnal monkey that eats and sleeps in the trees of the forest.",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/douc.jpg",
            "Population": 1300,
            "Latitude": 16.111648,
            "Longitude": 108.262122
        },
        {
            "Name": "Mooch",
            "Location": "Seattle",
            "Details": "An adorable Monkey who is traveling the world with Heather and live tweets his adventures @MotzMonkeys. Her favorite platform is iOS by far and is excited for the new iPhone 16!",
            "Image": "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/Mooch.PNG",
            "Population": 1,
            "Latitude": 47.608013,
            "Longitude": -122.335167
        }
    ]
    """;

    /// <summary>
    /// Gets all available monkey species from MonkeyMCP
    /// </summary>
    /// <returns>A read-only list of all monkeys</returns>
    public static IReadOnlyList<Monkey> GetMonkeys()
    {
        EnsureDataLoaded();
        IncrementAccess("GetMonkeys");
        return _monkeys!.AsReadOnly();
    }

    /// <summary>
    /// Gets a specific monkey by name (case-insensitive)
    /// </summary>
    /// <param name="name">The name of the monkey to find</param>
    /// <returns>The monkey if found, null otherwise</returns>
    public static Monkey? GetMonkeyByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        EnsureDataLoaded();
        IncrementAccess($"GetMonkeyByName:{name}");

        var monkey = _monkeys!.FirstOrDefault(m => 
            string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));

        if (monkey != null)
        {
            IncrementAccess($"MonkeyFound:{monkey.Name}");
        }

        return monkey;
    }

    /// <summary>
    /// Gets a random monkey from the collection
    /// </summary>
    /// <returns>A randomly selected monkey</returns>
    public static Monkey GetRandomMonkey()
    {
        EnsureDataLoaded();
        IncrementAccess("GetRandomMonkey");

        var random = new Random();
        var index = random.Next(_monkeys!.Count);
        var monkey = _monkeys[index];

        IncrementAccess($"RandomSelection:{monkey.Name}");
        return monkey;
    }

    /// <summary>
    /// Gets the total count of available monkeys
    /// </summary>
    /// <returns>The number of monkey species in the collection</returns>
    public static int GetMonkeyCount()
    {
        EnsureDataLoaded();
        IncrementAccess("GetMonkeyCount");
        return _monkeys!.Count;
    }

    /// <summary>
    /// Gets monkeys filtered by location (case-insensitive partial match)
    /// </summary>
    /// <param name="location">Location to search for</param>
    /// <returns>Monkeys found in the specified location</returns>
    public static IReadOnlyList<Monkey> GetMonkeysByLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return new List<Monkey>().AsReadOnly();

        EnsureDataLoaded();
        IncrementAccess($"GetMonkeysByLocation:{location}");

        var results = _monkeys!.Where(m => 
            m.Location.Contains(location, StringComparison.OrdinalIgnoreCase))
            .ToList();

        IncrementAccess($"LocationResults:{results.Count}");
        return results.AsReadOnly();
    }

    /// <summary>
    /// Gets monkeys with population above a certain threshold
    /// </summary>
    /// <param name="minPopulation">Minimum population count</param>
    /// <returns>Monkeys with population >= minPopulation</returns>
    public static IReadOnlyList<Monkey> GetMonkeysByMinPopulation(int minPopulation)
    {
        EnsureDataLoaded();
        IncrementAccess($"GetMonkeysByMinPopulation:{minPopulation}");

        var results = _monkeys!.Where(m => 
            m.Population.HasValue && m.Population.Value >= minPopulation)
            .ToList();

        IncrementAccess($"PopulationResults:{results.Count}");
        return results.AsReadOnly();
    }

    /// <summary>
    /// Gets access statistics for the MonkeyHelper
    /// </summary>
    /// <returns>Dictionary containing access counts by operation</returns>
    public static IReadOnlyDictionary<string, int> GetAccessCounts()
    {
        lock (_lock)
        {
            return new Dictionary<string, int>(_accessCounts);
        }
    }

    /// <summary>
    /// Gets the total number of accesses to MonkeyHelper methods
    /// </summary>
    /// <returns>Total access count</returns>
    public static int GetTotalAccessCount()
    {
        lock (_lock)
        {
            return _totalAccesses;
        }
    }

    /// <summary>
    /// Gets the most accessed monkey by name
    /// </summary>
    /// <returns>The name of the most accessed monkey and its access count</returns>
    public static (string? MonkeyName, int AccessCount) GetMostAccessedMonkey()
    {
        lock (_lock)
        {
            var monkeyAccesses = _accessCounts
                .Where(kvp => kvp.Key.StartsWith("MonkeyFound:") || kvp.Key.StartsWith("RandomSelection:"))
                .GroupBy(kvp => kvp.Key.Split(':')[1])
                .Select(g => new { MonkeyName = g.Key, AccessCount = g.Sum(x => x.Value) })
                .OrderByDescending(x => x.AccessCount)
                .FirstOrDefault();

            return (monkeyAccesses?.MonkeyName, monkeyAccesses?.AccessCount ?? 0);
        }
    }

    /// <summary>
    /// Resets all access counters
    /// </summary>
    public static void ResetAccessCounts()
    {
        lock (_lock)
        {
            _accessCounts.Clear();
            _totalAccesses = 0;
        }
    }

    /// <summary>
    /// Gets information about when the data was last loaded
    /// </summary>
    /// <returns>DateTime of last data load</returns>
    public static DateTime GetLastDataLoadTime()
    {
        return _lastDataLoad;
    }

    /// <summary>
    /// Forces a reload of monkey data from the source
    /// </summary>
    public static void RefreshData()
    {
        lock (_lock)
        {
            _monkeys = null;
            EnsureDataLoaded();
            IncrementAccess("RefreshData");
        }
    }

    /// <summary>
    /// Ensures that monkey data is loaded from MonkeyMCP
    /// </summary>
    private static void EnsureDataLoaded()
    {
        if (_monkeys == null)
        {
            lock (_lock)
            {
                if (_monkeys == null)
                {
                    _monkeys = LoadMonkeysFromMcp();
                    _lastDataLoad = DateTime.Now;
                }
            }
        }
    }

    /// <summary>
    /// Loads monkey data from MonkeyMCP source
    /// </summary>
    /// <returns>List of monkeys parsed from MCP data</returns>
    private static List<Monkey> LoadMonkeysFromMcp()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var monkeys = JsonSerializer.Deserialize<List<Monkey>>(_monkeyMcpData, options);
            return monkeys ?? new List<Monkey>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing MonkeyMCP data: {ex.Message}");
            return new List<Monkey>();
        }
    }

    /// <summary>
    /// Thread-safe method to increment access counters
    /// </summary>
    /// <param name="operation">The operation being tracked</param>
    private static void IncrementAccess(string operation)
    {
        lock (_lock)
        {
            _accessCounts[operation] = _accessCounts.GetValueOrDefault(operation, 0) + 1;
            _totalAccesses++;
        }
    }
}