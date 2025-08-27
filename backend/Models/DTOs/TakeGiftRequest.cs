using System.ComponentModel.DataAnnotations;

namespace WeddingGiftList.Api.Models.DTOs;

public class TakeGiftRequest
{
    [Required]
    [MaxLength(100)]
    public string GuestName { get; set; } = string.Empty;
    
    /// <summary>
    /// Row version for optimistic concurrency control
    /// </summary>
    public byte[]? RowVersion { get; set; }
}
