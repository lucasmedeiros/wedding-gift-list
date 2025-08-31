using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingGiftList.Api.Models.DTOs;
using WeddingGiftList.Api.Services;

namespace WeddingGiftList.Api.Controllers;

/// <summary>
/// Controller for managing wedding gifts
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Gifts")]
public class GiftsController(ILogger<GiftsController> logger, IGiftsService  giftsService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves all gifts in the wedding gift list
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A list of all gifts with their current status</returns>
    /// <response code="200">Returns the list of gifts</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GiftResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
    /// Creates a new gift in the wedding gift list
    /// </summary>
    /// <param name="request">The gift creation request containing name and description</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The newly created gift</returns>
    /// <response code="201">Returns the newly created gift</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(GiftResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
    /// Update a existing gift in the wedding gift list
    /// </summary>
    /// <param name="request">The gift creation request containing name and description</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The newly created gift</returns>
    /// <response code="201">Returns the newly created gift</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GiftResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GiftResponse>> UpdateGift(int id, UpdateGiftRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await giftsService.UpdateAsync(id, request, cancellationToken);

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
            logger.LogError(ex, "Error creating gift");
            return StatusCode(500, "An error occurred while creating the gift");
        }
    }

    /// <summary>
    /// Takes a gift (marks it as taken by a guest)
    /// </summary>
    /// <param name="id">The unique identifier of the gift to take</param>
    /// <param name="request">The take gift request containing guest name and optional version</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The updated gift with the guest's information</returns>
    /// <response code="200">Returns the updated gift</response>
    /// <response code="400">If the request is invalid or gift is already taken</response>
    /// <response code="404">If the gift with the specified ID was not found</response>
    /// <response code="409">If there was a concurrency conflict</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost("{id}/take")]
    [ProducesResponseType(typeof(GiftResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
    /// Releases a gift (makes it available again)
    /// </summary>
    /// <param name="id">The unique identifier of the gift to release</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>The updated gift with cleared guest information</returns>
    /// <response code="200">Returns the updated gift</response>
    /// <response code="400">If the gift is not currently taken</response>
    /// <response code="404">If the gift with the specified ID was not found</response>
    /// <response code="409">If there was a concurrency conflict</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost("{id}/release")]
    [ProducesResponseType(typeof(GiftResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Deletes a gift from the wedding gift list
    /// </summary>
    /// <param name="id">The unique identifier of the gift to delete</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">Gift was successfully deleted</response>
    /// <response code="404">If the gift with the specified ID was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteGift(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await giftsService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound($"Gift with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting gift {GiftId}", id);
            return StatusCode(500, "An error occurred while deleting the gift");
        }
    }
}
