using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingGiftList.Api.Data;
using WeddingGiftList.Api.Models;
using WeddingGiftList.Api.Models.DTOs;

namespace WeddingGiftList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GiftsController : ControllerBase
{
    private readonly WeddingGiftListContext _context;
    private readonly ILogger<GiftsController> _logger;

    public GiftsController(WeddingGiftListContext context, ILogger<GiftsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all gifts with their current status
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiftResponse>>> GetGifts()
    {
        try
        {
            var gifts = await _context.Gifts.ToListAsync();
            var giftResponses = gifts.Select(g => new GiftResponse
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                ImageUrl = g.ImageUrl,
                TakenByGuestName = g.TakenByGuestName,
                TakenAt = g.TakenAt,
                IsTaken = g.IsTaken,
                RowVersion = g.RowVersion
            });

            return Ok(giftResponses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving gifts");
            return StatusCode(500, "An error occurred while retrieving gifts");
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
            var gift = await _context.Gifts.FindAsync(id);
            if (gift == null)
            {
                return NotFound($"Gift with ID {id} not found");
            }

            if (gift.IsTaken)
            {
                return Conflict(new { message = "Gift has already been taken", takenBy = gift.TakenByGuestName });
            }

            // Update the gift
            gift.TakenByGuestName = request.GuestName;
            gift.TakenAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new GiftResponse
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                ImageUrl = gift.ImageUrl,
                TakenByGuestName = gift.TakenByGuestName,
                TakenAt = gift.TakenAt,
                IsTaken = gift.IsTaken,
                RowVersion = gift.RowVersion
            };

            return Ok(response);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "The gift was modified by another user. Please refresh and try again." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error taking gift {GiftId}", id);
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
            var gift = await _context.Gifts.FindAsync(id);
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

            await _context.SaveChangesAsync();

            var response = new GiftResponse
            {
                Id = gift.Id,
                Name = gift.Name,
                Description = gift.Description,
                ImageUrl = gift.ImageUrl,
                TakenByGuestName = gift.TakenByGuestName,
                TakenAt = gift.TakenAt,
                IsTaken = gift.IsTaken,
                RowVersion = gift.RowVersion
            };

            return Ok(response);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "The gift was modified by another user. Please refresh and try again." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing gift {GiftId}", id);
            return StatusCode(500, "An error occurred while releasing the gift");
        }
    }
}
