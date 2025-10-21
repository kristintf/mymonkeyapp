namespace MyMonkeyApp.Models;

/// <summary>
/// Represents a monkey species with its characteristics and habitat information
/// Based on MonkeyMCP data structure
/// </summary>
public class Monkey
{
    /// <summary>
    /// The name of the monkey species
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The natural habitat or location where this monkey species is found
    /// </summary>
    public required string Location { get; init; }

    /// <summary>
    /// The estimated population count of this species (null if unknown)
    /// </summary>
    public int? Population { get; init; }

    /// <summary>
    /// Additional details and characteristics about the monkey species
    /// </summary>
    public required string Details { get; init; }

    /// <summary>
    /// URL to an image of the monkey species
    /// </summary>
    public string? Image { get; init; }

    /// <summary>
    /// Geographic latitude coordinate of the monkey's habitat
    /// </summary>
    public double? Latitude { get; init; }

    /// <summary>
    /// Geographic longitude coordinate of the monkey's habitat
    /// </summary>
    public double? Longitude { get; init; }

    /// <summary>
    /// Gets the geographic coordinates as a formatted string
    /// </summary>
    public string? Coordinates => 
        Latitude.HasValue && Longitude.HasValue 
            ? $"{Latitude:F6}, {Longitude:F6}" 
            : null;

    /// <summary>
    /// Returns a string representation of the monkey
    /// </summary>
    public override string ToString()
    {
        var populationText = Population.HasValue ? $"{Population:N0}" : "Unknown";
        return $"{Name} - {Location} (Population: {populationText})";
    }

    /// <summary>
    /// Returns a detailed string representation including all properties
    /// </summary>
    public string ToDetailedString()
    {
        var result = $"🐵 {Name}\n";
        result += $"📍 Location: {Location}\n";
        result += $"👥 Population: {(Population?.ToString("N0") ?? "Unknown")}\n";
        result += $"📝 Details: {Details}\n";
        
        if (!string.IsNullOrEmpty(Coordinates))
        {
            result += $"🗺️ Coordinates: {Coordinates}\n";
        }
        
        if (!string.IsNullOrEmpty(Image))
        {
            result += $"🖼️ Image: {Image}\n";
        }
        
        return result;
    }
}