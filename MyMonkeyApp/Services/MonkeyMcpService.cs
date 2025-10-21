using System.Text.Json;
using MyMonkeyApp.Models;

namespace MyMonkeyApp.Services;

/// <summary>
/// Service for interacting with MonkeyMCP data
/// Note: This is a simulation of MCP integration - in a real scenario, 
/// you would integrate with the actual MCP server protocol
/// </summary>
public static class MonkeyMcpService
{
    /// <summary>
    /// Sample MonkeyMCP data (simulating the actual MCP server response)
    /// In a real implementation, this would come from the MCP server
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

    private static List<Monkey>? _cachedMonkeys;

    /// <summary>
    /// Gets all monkeys from MonkeyMCP
    /// </summary>
    /// <returns>A read-only list of monkeys from MonkeyMCP</returns>
    public static IReadOnlyList<Monkey> GetMonkeys()
    {
        if (_cachedMonkeys == null)
        {
            _cachedMonkeys = LoadMonkeysFromMcp();
        }
        return _cachedMonkeys.AsReadOnly();
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

        var monkeys = GetMonkeys();
        return monkeys.FirstOrDefault(m => 
            string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets a random monkey from MonkeyMCP
    /// </summary>
    /// <returns>A randomly selected monkey</returns>
    public static Monkey GetRandomMonkey()
    {
        var monkeys = GetMonkeys();
        var random = new Random();
        var index = random.Next(monkeys.Count);
        return monkeys[index];
    }

    /// <summary>
    /// Gets the total count of monkeys available from MonkeyMCP
    /// </summary>
    /// <returns>The number of monkey species in the collection</returns>
    public static int GetMonkeyCount()
    {
        return GetMonkeys().Count;
    }

    /// <summary>
    /// Filters monkeys by location (case-insensitive partial match)
    /// </summary>
    /// <param name="location">Location to search for</param>
    /// <returns>Monkeys found in the specified location</returns>
    public static IReadOnlyList<Monkey> GetMonkeysByLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return new List<Monkey>().AsReadOnly();

        var monkeys = GetMonkeys();
        return monkeys.Where(m => 
            m.Location.Contains(location, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Gets monkeys with population above a certain threshold
    /// </summary>
    /// <param name="minPopulation">Minimum population count</param>
    /// <returns>Monkeys with population >= minPopulation</returns>
    public static IReadOnlyList<Monkey> GetMonkeysByMinPopulation(int minPopulation)
    {
        var monkeys = GetMonkeys();
        return monkeys.Where(m => 
            m.Population.HasValue && m.Population.Value >= minPopulation)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Loads monkey data from MonkeyMCP (simulated)
    /// In a real implementation, this would make actual MCP calls
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
}