using System.ComponentModel.DataAnnotations;

namespace WeddingGiftList.Api.Models.DTOs;

public class CreateGiftRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
}
