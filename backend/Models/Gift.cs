using System.ComponentModel.DataAnnotations;

namespace WeddingGiftList.Api.Models;

public class Gift
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? TakenByGuestName { get; set; }
    
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
