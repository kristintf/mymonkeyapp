using MyMonkeyApp.Helpers;

namespace MyMonkeyApp;

/// <summary>
/// Interactive console application for exploring monkey species data
/// </summary>
public class Program
{
    private static readonly string MonkeyAsciiArt = @"
            🐵
          .=""=.
        _/.-.-.\_ 
       ( ( o o ) )
        |/  ""  \|
         \---/
         /```\
        / /_,_\ \
       \_\\\_/_/
        /-----\
    ";

    private static readonly string WelcomeArt = @"
    🌟========================================🌟
    🐒            MONKEY APP 2025             🐒
    🍌     Explore Amazing Monkey Species!    🍌
    🌟========================================🌟
    ";

    public static void Main(string[] args)
    {
        DisplayWelcome();
        RunInteractiveMenu();
        DisplayGoodbye();
    }

    /// <summary>
    /// Displays the welcome message and initializes the app
    /// </summary>
    private static void DisplayWelcome()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(WelcomeArt);
        Console.ResetColor();
        Console.WriteLine();
        
        // Initialize data and show basic info
        var totalMonkeys = MonkeyHelper.GetMonkeyCount();
        Console.WriteLine($"📊 Loaded {totalMonkeys} monkey species from MonkeyMCP");
        Console.WriteLine($"📅 Data loaded at: {MonkeyHelper.GetLastDataLoadTime():yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    /// <summary>
    /// Main interactive menu loop
    /// </summary>
    private static void RunInteractiveMenu()
    {
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.Clear();
            DisplayMainMenu();

            var choice = GetUserChoice();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ListAllMonkeys();
                    break;
                case "2":
                    GetMonkeyByName();
                    break;
                case "3":
                    GetRandomMonkey();
                    break;
                case "4":
                    DisplayAccessStats();
                    break;
                case "5":
                    keepRunning = false;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Invalid choice. Please select 1-5.");
                    Console.ResetColor();
                    PauseForUser();
                    break;
            }
        }
    }

    /// <summary>
    /// Displays the main menu options
    /// </summary>
    private static void DisplayMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("🐵 MONKEY APP - MAIN MENU 🐵");
        Console.WriteLine("================================");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("1️⃣  📋 List all monkeys");
        Console.WriteLine("2️⃣  🔍 Get details for a specific monkey");
        Console.WriteLine("3️⃣  🎲 Get a random monkey");
        Console.WriteLine("4️⃣  📊 View access statistics");
        Console.WriteLine("5️⃣  🚪 Exit");
        Console.WriteLine();
        Console.Write("👉 Enter your choice (1-5): ");
    }

    /// <summary>
    /// Gets user input for menu choice
    /// </summary>
    /// <returns>User's menu selection</returns>
    private static string GetUserChoice()
    {
        var input = Console.ReadLine()?.Trim() ?? "";
        return input;
    }

    /// <summary>
    /// Lists all available monkeys
    /// </summary>
    private static void ListAllMonkeys()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("📋 ALL MONKEY SPECIES");
        Console.WriteLine("====================");
        Console.ResetColor();
        Console.WriteLine();

        var monkeys = MonkeyHelper.GetMonkeys();
        
        for (int i = 0; i < monkeys.Count; i++)
        {
            var monkey = monkeys[i];
            Console.WriteLine($"{i + 1:D2}. {monkey.Name}");
            Console.WriteLine($"    📍 Location: {monkey.Location}");
            Console.WriteLine($"    👥 Population: {monkey.Population?.ToString("N0") ?? "Unknown"}");
            
            if (!string.IsNullOrEmpty(monkey.Coordinates))
            {
                Console.WriteLine($"    🗺️  Coordinates: {monkey.Coordinates}");
            }
            
            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"📊 Total: {monkeys.Count} monkey species");
        Console.ResetColor();
        
        PauseForUser();
    }

    /// <summary>
    /// Gets details for a specific monkey by name
    /// </summary>
    private static void GetMonkeyByName()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("🔍 SEARCH FOR MONKEY BY NAME");
        Console.WriteLine("=============================");
        Console.ResetColor();
        Console.WriteLine();
        Console.Write("👉 Enter monkey name: ");
        
        var name = Console.ReadLine()?.Trim() ?? "";
        
        if (string.IsNullOrEmpty(name))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Please enter a monkey name.");
            Console.ResetColor();
            PauseForUser();
            return;
        }

        var monkey = MonkeyHelper.GetMonkeyByName(name);
        
        if (monkey == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Monkey '{name}' not found.");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("💡 Try searching for one of these monkeys:");
            
            var allMonkeys = MonkeyHelper.GetMonkeys();
            var suggestions = allMonkeys
                .Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Take(5);
            
            foreach (var suggestion in suggestions)
            {
                Console.WriteLine($"   • {suggestion.Name}");
            }
            
            if (!suggestions.Any())
            {
                Console.WriteLine("   • Proboscis Monkey");
                Console.WriteLine("   • Japanese Macaque");
                Console.WriteLine("   • Sebastian");
            }
        }
        else
        {
            DisplayMonkeyDetails(monkey);
        }
        
        PauseForUser();
    }

    /// <summary>
    /// Gets and displays a random monkey
    /// </summary>
    private static void GetRandomMonkey()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("🎲 RANDOM MONKEY SELECTION");
        Console.WriteLine("==========================");
        Console.ResetColor();
        Console.WriteLine();

        var monkey = MonkeyHelper.GetRandomMonkey();
        Console.WriteLine("🎉 Here's your random monkey:");
        Console.WriteLine();
        
        DisplayMonkeyDetails(monkey);
        PauseForUser();
    }

    /// <summary>
    /// Displays detailed information about a specific monkey with ASCII art
    /// </summary>
    /// <param name="monkey">The monkey to display</param>
    private static void DisplayMonkeyDetails(MyMonkeyApp.Models.Monkey monkey)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(MonkeyAsciiArt);
        Console.ResetColor();
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"🐵 {monkey.Name.ToUpper()}");
        Console.WriteLine(new string('=', monkey.Name.Length + 3));
        Console.ResetColor();
        Console.WriteLine();
        
        Console.WriteLine($"📍 Location: {monkey.Location}");
        Console.WriteLine($"👥 Population: {monkey.Population?.ToString("N0") ?? "Unknown"}");
        
        if (!string.IsNullOrEmpty(monkey.Coordinates))
        {
            Console.WriteLine($"🗺️ Coordinates: {monkey.Coordinates}");
        }
        
        if (!string.IsNullOrEmpty(monkey.Image))
        {
            Console.WriteLine($"🖼️ Image: {monkey.Image}");
        }
        
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("📝 Details:");
        Console.ResetColor();
        Console.WriteLine(monkey.Details);
        Console.WriteLine();
    }

    /// <summary>
    /// Displays access statistics
    /// </summary>
    private static void DisplayAccessStats()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("📊 ACCESS STATISTICS");
        Console.WriteLine("===================");
        Console.ResetColor();
        Console.WriteLine();

        var totalAccesses = MonkeyHelper.GetTotalAccessCount();
        Console.WriteLine($"📈 Total API Calls: {totalAccesses}");
        
        var (mostAccessedMonkey, accessCount) = MonkeyHelper.GetMostAccessedMonkey();
        if (mostAccessedMonkey != null)
        {
            Console.WriteLine($"🏆 Most Popular Monkey: {mostAccessedMonkey} ({accessCount} times)");
        }
        
        Console.WriteLine($"📅 Data Last Loaded: {MonkeyHelper.GetLastDataLoadTime():yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine();
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("🔍 Recent Access Breakdown:");
        Console.ResetColor();
        
        var accessCounts = MonkeyHelper.GetAccessCounts();
        var topAccesses = accessCounts
            .OrderByDescending(x => x.Value)
            .Take(10);
        
        foreach (var kvp in topAccesses)
        {
            var operation = kvp.Key;
            var count = kvp.Value;
            
            // Format operation name for better readability
            if (operation.StartsWith("MonkeyFound:"))
            {
                operation = $"🐵 Found: {operation.Split(':')[1]}";
            }
            else if (operation.StartsWith("RandomSelection:"))
            {
                operation = $"🎲 Random: {operation.Split(':')[1]}";
            }
            else if (operation.StartsWith("GetMonkeyByName:"))
            {
                operation = $"🔍 Search: {operation.Split(':')[1]}";
            }
            
            Console.WriteLine($"  • {operation}: {count} times");
        }
        
        Console.WriteLine();
        PauseForUser();
    }

    /// <summary>
    /// Pauses execution and waits for user input
    /// </summary>
    private static void PauseForUser()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Press any key to continue...");
        Console.ResetColor();
        Console.ReadKey();
    }

    /// <summary>
    /// Displays goodbye message with final statistics
    /// </summary>
    private static void DisplayGoodbye()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("🌟 THANK YOU FOR USING MONKEY APP! 🌟");
        Console.WriteLine("====================================");
        Console.ResetColor();
        Console.WriteLine();
        
        Console.WriteLine("📊 Final Session Statistics:");
        Console.WriteLine($"   • Total API Calls: {MonkeyHelper.GetTotalAccessCount()}");
        
        var (mostAccessedMonkey, accessCount) = MonkeyHelper.GetMostAccessedMonkey();
        if (mostAccessedMonkey != null)
        {
            Console.WriteLine($"   • Most Popular Monkey: {mostAccessedMonkey} ({accessCount} times)");
        }
        
        Console.WriteLine($"   • Monkeys Available: {MonkeyHelper.GetMonkeyCount()}");
        Console.WriteLine();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("🐵 Hope you enjoyed learning about monkeys! 🐵");
        Console.WriteLine("🍌 Come back anytime to explore more! 🍌");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
