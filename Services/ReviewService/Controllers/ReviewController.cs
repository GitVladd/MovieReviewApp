using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Dtos;
using ReviewService.Service;
using System.Security.Claims;

namespace ReviewService.Controllers
{
    [ApiController]
    [Route("api")]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewService _reviewService;

        public ReviewController(
            ILogger<ReviewController> logger,
            IReviewService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }

        [HttpGet("movies/{movieId:Guid}/reviews")]
        public async Task<ActionResult<List<ReviewGetDto>>> GetReviewsForMovie(Guid movieId)
        {

            var result = await _reviewService.GetReviewsByMovieIdAsync(movieId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("reviews/{id:Guid}")]
        public async Task<ActionResult<ReviewGetDto>> GetById(Guid id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpPost("reviews")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ReviewGetDto>> Create([FromBody] ReviewCreateDto createDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null) return Unauthorized();

            var userId = Guid.Parse(userIdString);

            var createdReview = await _reviewService.CreateAsync(createDto, userId);
            return CreatedAtAction(nameof(GetById), new { id = createdReview.Id }, createdReview);
        }

        [HttpPut("reviews/{id:Guid}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ReviewGetDto>> Update(Guid id, [FromBody] ReviewUpdateDto updateDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null) return Unauthorized();
            var userId = Guid.Parse(userIdString);

            var updatedReview = await _reviewService.UpdateAsync(id, updateDto, userId);
            if (updatedReview == null)
                return NotFound();

            return Ok(updatedReview);
        }

        [HttpDelete("reviews/{id:Guid}")]
        [Authorize(Roles = "Admin, Moderator, User")]
        public async Task<ActionResult> Delete(Guid reviewId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null) return Unauthorized();

            var userId = Guid.Parse(userIdString);
            var review = await _reviewService.GetByIdAsync(reviewId);
            if (review == null) return NotFound();

            await _reviewService.DeleteAsync(reviewId, userId);

            return NoContent();
        }
    }
}
