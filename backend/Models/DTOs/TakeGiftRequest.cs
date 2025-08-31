using System.ComponentModel.DataAnnotations;

namespace WeddingGiftList.Api.Models.DTOs;

/// <summary>
/// Request model for taking a gift
/// </summary>
/// <example>
/// {
///   "guestName": "Alice Johnson",
///   "version": 1
/// }
/// </example>
public class TakeGiftRequest
{
    /// <summary>
    /// Name of the guest taking the gift (required, max 100 characters)
    /// </summary>
    /// <example>Alice Johnson</example>
    [Required]
    [MaxLength(100)]
    public string GuestName { get; set; } = string.Empty;
    
    /// <summary>
    /// Version for optimistic concurrency control (optional). 
    /// If provided, the request will fail if the gift has been modified by another user.
    /// </summary>
    /// <example>1</example>
    public int? Version { get; set; }
}
