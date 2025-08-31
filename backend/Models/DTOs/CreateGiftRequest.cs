using System.ComponentModel.DataAnnotations;

namespace WeddingGiftList.Api.Models.DTOs;

/// <summary>
/// Request model for creating a new gift
/// </summary>
/// <example>
/// {
///   "name": "Coffee Maker",
///   "description": "A premium automatic coffee maker with built-in grinder"
/// }
/// </example>
public class CreateGiftRequest
{
    /// <summary>
    /// Name of the gift (required, max 200 characters)
    /// </summary>
    /// <example>Coffee Maker</example>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed description of the gift (optional, max 1000 characters)
    /// </summary>
    /// <example>A premium automatic coffee maker with built-in grinder</example>
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
}
