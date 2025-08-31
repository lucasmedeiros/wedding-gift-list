using System.ComponentModel.DataAnnotations;

namespace WeddingGiftList.Api.Models;

/// <summary>
/// Represents a gift in the wedding gift list with all its details and status
/// </summary>
public class Gift
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
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed description of the gift
    /// </summary>
    /// <example>A high-quality kitchen blender for smoothies and cooking</example>
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Name of the guest who has taken this gift, null if available
    /// </summary>
    /// <example>John Smith</example>
    [MaxLength(100)]
    public string? TakenByGuestName { get; set; }
    
    /// <summary>
    /// DateTime when the gift was taken, null if available
    /// </summary>
    /// <example>2024-01-15T10:30:00Z</example>
    public DateTime? TakenAt { get; set; }
    
    /// <summary>
    /// Version for optimistic concurrency control
    /// </summary>
    [ConcurrencyCheck]
    public int Version { get; set; } = 1;
    
    /// <summary>
    /// Indicates if the gift has been taken by a guest
    /// </summary>
    public bool IsTaken => !string.IsNullOrEmpty(TakenByGuestName);
}
