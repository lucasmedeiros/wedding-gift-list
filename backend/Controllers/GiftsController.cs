using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingGiftList.Api.Data;
using WeddingGiftList.Api.Models;
using WeddingGiftList.Api.Models.DTOs;

namespace WeddingGiftList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GiftsController(WeddingGiftListContext context, ILogger<GiftsController> logger)
    : ControllerBase
{
    /// <summary>
    /// Get all gifts with their current status
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiftResponse>>> GetGifts()
    {
        try
        {
            var gifts = await context.Gifts.ToListAsync();
            var giftResponses = gifts.Select(g => new GiftResponse
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                TakenByGuestName = g.TakenByGuestName,
                TakenAt = g.TakenAt,
                IsTaken = g.IsTaken,
                Version = g.Version
            });

            return Ok(giftResponses);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving gifts");
            return StatusCode(500, "An error occurred while retrieving gifts");
        }
    }

    /// <summary>
    /// Create a new gift
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<GiftResponse>> CreateGift(CreateGiftRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var gift = new Gift
            {
                Name = request.Name,
                Description = request.Description,
                Version = 1
            };

            context.Gifts.Add(gift);
            await context.SaveChangesAsync();

            var response = new GiftResponse
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                TakenByGuestName = gift.TakenByGuestName,
                TakenAt = gift.TakenAt,
                IsTaken = gift.IsTaken,
                Version = gift.Version
            };

            return CreatedAtAction(nameof(GetGifts), new { id = gift.Id }, response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating gift");
            return StatusCode(500, "An error occurred while creating the gift");
        }
    }

    /// <summary>
    /// Take a gift (mark as taken by a guest)
    /// </summary>
    [HttpPost("{id}/take")]
    public async Task<ActionResult<GiftResponse>> TakeGift(int id, TakeGiftRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var gift = await context.Gifts.FindAsync(id);
            if (gift == null)
            {
                return NotFound($"Gift with ID {id} not found");
            }

            if (gift.IsTaken)
            {
                return Conflict(new { message = "Gift has already been taken", takenBy = gift.TakenByGuestName });
            }

            // Check version for optimistic concurrency (if provided)
            if (request.Version.HasValue && gift.Version != request.Version.Value)
            {
                return Conflict(new { message = "The gift was modified by another user. Please refresh and try again." });
            }

            // Update the gift
            gift.TakenByGuestName = request.GuestName;
            gift.TakenAt = DateTime.UtcNow;
            gift.Version++; // Increment version for concurrency control

            await context.SaveChangesAsync();

            var response = new GiftResponse
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                TakenByGuestName = gift.TakenByGuestName,
                TakenAt = gift.TakenAt,
                IsTaken = gift.IsTaken,
                Version = gift.Version
            };

            return Ok(response);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "The gift was modified by another user. Please refresh and try again." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error taking gift {GiftId}", id);
            return StatusCode(500, "An error occurred while taking the gift");
        }
    }

    /// <summary>
    /// Release a gift (make it available again)
    /// </summary>
    [HttpPost("{id}/release")]
    public async Task<ActionResult<GiftResponse>> ReleaseGift(int id)
    {
        try
        {
            var gift = await context.Gifts.FindAsync(id);
            if (gift == null)
            {
                return NotFound($"Gift with ID {id} not found");
            }

            if (!gift.IsTaken)
            {
                return BadRequest("Gift is not currently taken");
            }

            // Release the gift
            gift.TakenByGuestName = null;
            gift.TakenAt = null;
            gift.Version++; // Increment version for concurrency control

            await context.SaveChangesAsync();

            var response = new GiftResponse
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                TakenByGuestName = gift.TakenByGuestName,
                TakenAt = gift.TakenAt,
                IsTaken = gift.IsTaken,
                Version = gift.Version
            };

            return Ok(response);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "The gift was modified by another user. Please refresh and try again." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error releasing gift {GiftId}", id);
            return StatusCode(500, "An error occurred while releasing the gift");
        }
    }
}
