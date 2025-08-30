using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingGiftList.Api.Models.DTOs;
using WeddingGiftList.Api.Services;

namespace WeddingGiftList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GiftsController(ILogger<GiftsController> logger, IGiftsService  giftsService)
    : ControllerBase
{
    /// <summary>
    /// Get all gifts with their current status
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiftResponse>>> GetGifts(CancellationToken cancellationToken)
    {
        try
        {
            var gifts = await giftsService.ListAsync(cancellationToken);
            return Ok(gifts);
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
    public async Task<ActionResult<GiftResponse>> CreateGift(CreateGiftRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await giftsService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(CreateGift), new { id = response.Id }, response);
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
    public async Task<ActionResult<GiftResponse>> TakeGift(int id, TakeGiftRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await giftsService.TakeAsync(id, request, cancellationToken);
            if (response == null)
            {
                return NotFound($"Gift with ID {id} not found");
            }

            return Ok(response);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "The gift was modified by another user. Please refresh and try again." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
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
    public async Task<ActionResult<GiftResponse>> ReleaseGift(int id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await giftsService.ReleaseAsync(id, cancellationToken);
            if (response == null)
            {
                return NotFound($"Gift with ID {id} not found");
            }

            return Ok(response);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "The gift was modified by another user. Please refresh and try again." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error releasing gift {GiftId}", id);
            return StatusCode(500, "An error occurred while releasing the gift");
        }
    }
}
