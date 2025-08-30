namespace WeddingGiftList.Api.Models.DTOs;

public class GiftResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string? TakenByGuestName { get; set; }
    public DateTime? TakenAt { get; set; }
    public bool IsTaken { get; set; }
    public int Version { get; set; }
}
