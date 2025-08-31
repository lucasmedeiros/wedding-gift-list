namespace WeddingGiftList.Api.Models.DTOs;

/// <summary>
/// Represents a gift response in the wedding gift list with all its details and status
/// </summary>
public class GiftResponse
{
    /// <summary>
    /// Unique identifier for the gift
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }
    
    /// <summary>
    /// Name of the gift
    /// </summary>
    /// <example>Blender</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed description of the gift
    /// </summary>
    /// <example>A high-quality kitchen blender for smoothies and cooking</example>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Name of the guest who has taken this gift, null if available
    /// </summary>
    /// <example>John Smith</example>
    public string? TakenByGuestName { get; set; }
    
    /// <summary>
    /// DateTime when the gift was taken, null if available
    /// </summary>
    /// <example>2024-01-15T10:30:00Z</example>
    public DateTime? TakenAt { get; set; }
    
    /// <summary>
    /// Indicates whether the gift has been taken by a guest
    /// </summary>
    /// <example>true</example>
    public bool IsTaken { get; set; }
    
    /// <summary>
    /// Version number for optimistic concurrency control
    /// </summary>
    /// <example>2</example>
    public int Version { get; set; }
}
