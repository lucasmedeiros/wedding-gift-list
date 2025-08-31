using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WeddingGiftList.Api.Data;
using WeddingGiftList.Api.Models;
using WeddingGiftList.Api.Models.DTOs;

namespace WeddingGiftList.Api.Services;

public class GiftsService(WeddingGiftListContext context, IMemoryCache memoryCache)
    : IGiftsService
{
    private static string BuildGiftByIdCacheKey(int id) => $"cache_gift_id_{id}";
    private static string BuildGiftsCacheKey() => "cached_gifts";
    
    public async Task<List<GiftResponse>> ListAsync(CancellationToken cancellationToken)
    {
        var cacheKey = BuildGiftsCacheKey();
        
        if (memoryCache.TryGetValue(BuildGiftsCacheKey(), out List<GiftResponse>? gifts))
        {
            if (gifts != null)
            {
                return gifts;
            }
        }

        var giftsResponsesFromDb = await ListGiftsInternalAsync(cancellationToken);
        var cacheEntryOptions = new MemoryCacheEntryOptions();
        memoryCache.Set(cacheKey, giftsResponsesFromDb, cacheEntryOptions);
        
        return giftsResponsesFromDb;
    }

    private async Task<List<GiftResponse>> ListGiftsInternalAsync(CancellationToken cancellationToken)
    {
        var giftsFromDb = await context.Gifts.ToListAsync(cancellationToken);
        var giftResponses = giftsFromDb.Select(g => new GiftResponse
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            TakenByGuestName = g.TakenByGuestName,
            TakenAt = g.TakenAt,
            IsTaken = g.IsTaken,
            Version = g.Version
        }).ToList();
        return giftResponses;
    }

    public async Task<GiftResponse?> FindAsync(int id, CancellationToken cancellationToken)
    {
        var gift = await FindInternalAsync(id, cancellationToken);

        if (gift == null)
        {
            return null;
        }
        
        return new GiftResponse
        {
            Id = gift.Id,
            Name = gift.Name,
            Description = gift.Description,
            TakenByGuestName = gift.TakenByGuestName,
            TakenAt = gift.TakenAt,
            IsTaken = gift.IsTaken,
            Version = gift.Version
        };
    }

    public async Task<GiftResponse> CreateAsync(CreateGiftRequest request, CancellationToken cancellationToken)
    {
        var gift = new Gift
        {
            Name = request.Name,
            Description = request.Description,
            Version = 0
        };

        context.Gifts.Add(gift);
        await context.SaveChangesAsync(cancellationToken);
        UpdateGiftCache(gift);
        DestroyGiftsCache();
        
        return new GiftResponse
        {
            Id = gift.Id,
            Name = gift.Name,
            Description = gift.Description,
            TakenByGuestName = gift.TakenByGuestName,
            TakenAt = gift.TakenAt,
            IsTaken = gift.IsTaken,
            Version = gift.Version
        };
    }

    private async Task<Gift?> FindInternalAsync(int id, CancellationToken cancellationToken)
    {
        var cacheKey = BuildGiftByIdCacheKey(id);
        if (memoryCache.TryGetValue(cacheKey, out Gift? gift))
        {
            return gift;
        }
        
        var giftFromDb = await context.Gifts.FindAsync([id], cancellationToken: cancellationToken);
        memoryCache.Set(cacheKey, giftFromDb);
        
        return giftFromDb;
    }

    public async Task<GiftResponse?> TakeAsync(int id, TakeGiftRequest request, CancellationToken cancellationToken)
    {
        var gift = await FindInternalAsync(id, cancellationToken);
        if (gift == null)
        {
            return null;
        }

        if (gift.IsTaken)
        {
            throw new InvalidOperationException($"Gift {id} is already taken. takenBy = {gift.TakenByGuestName}");
        }
        
        if (request.Version.HasValue && gift.Version != request.Version.Value)
        {
            throw new InvalidOperationException("The gift was modified by another user. Please refresh and try again.");
        }
        
        gift.TakenByGuestName = request.GuestName;
        gift.TakenAt = DateTime.UtcNow;
        gift.Version++; // Increment version for concurrency control
        await context.SaveChangesAsync(cancellationToken);
        UpdateGiftCache(gift);
        DestroyGiftsCache();

        return new GiftResponse
        {
            Id = gift.Id,
            Name = gift.Name,
            Description = gift.Description,
            TakenByGuestName = gift.TakenByGuestName,
            TakenAt = gift.TakenAt,
            IsTaken = gift.IsTaken,
            Version = gift.Version
        };
    }

    public async Task<GiftResponse?> ReleaseAsync(int id, CancellationToken cancellationToken)
    {
        var gift = await FindInternalAsync(id, cancellationToken);
        if (gift == null)
        {
            return null;
        }

        if (!gift.IsTaken)
        {
            throw new InvalidOperationException($"Gift {id} is not currently taken.");
        }
        
        // Release the gift
        gift.TakenByGuestName = null;
        gift.TakenAt = null;
        gift.Version++; // Increment version for concurrency control
        await context.SaveChangesAsync(cancellationToken);
        UpdateGiftCache(gift);
        DestroyGiftsCache();
        
        return new GiftResponse
        {
            Id = gift.Id,
            Name = gift.Name,
            Description = gift.Description,
            TakenByGuestName = gift.TakenByGuestName,
            TakenAt = gift.TakenAt,
            IsTaken = gift.IsTaken,
            Version = gift.Version
        };
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var gift = await FindInternalAsync(id, cancellationToken);
        if (gift == null)
        {
            return false;
        }

        context.Gifts.Remove(gift);
        await context.SaveChangesAsync(cancellationToken);
        DestroyGiftCache(gift.Id);
        DestroyGiftsCache();

        return true;
    }
    
    private void DestroyGiftsCache()
    {
        var cacheKey = BuildGiftsCacheKey();
        memoryCache.Remove(cacheKey);
    }

    private void DestroyGiftCache(int id)
    {
        var cacheKey = BuildGiftByIdCacheKey(id);
        memoryCache.Remove(cacheKey);
    }

    private void UpdateGiftCache(Gift gift)
    {
        var cacheKey = BuildGiftByIdCacheKey(gift.Id);

        var options = BuildCacheEntryOptions();

        memoryCache.Set(cacheKey, gift, options);
    }

    private static MemoryCacheEntryOptions BuildCacheEntryOptions()
    {
        return new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
            .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            .SetPriority(CacheItemPriority.Normal);
    }
}