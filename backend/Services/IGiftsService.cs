using WeddingGiftList.Api.Models;
using WeddingGiftList.Api.Models.DTOs;

namespace WeddingGiftList.Api.Services;

public interface IGiftsService
{
    Task<List<GiftResponse>> ListAsync(CancellationToken cancellationToken);
    Task<GiftResponse?> FindAsync(int id, CancellationToken cancellationToken);
    Task<GiftResponse> CreateAsync(CreateGiftRequest request, CancellationToken cancellationToken);
    Task<GiftResponse?> TakeAsync(int id, TakeGiftRequest request, CancellationToken cancellationToken);
    Task<GiftResponse?> ReleaseAsync(int id, CancellationToken cancellationToken);
}