using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Dtos;
using ReviewService.Service;

namespace ReviewService.Controllers
{
    [ApiController]
    [Route("api/reviews")]
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

        [HttpGet("movies/{movieId:Guid}")]
        public async Task<ActionResult<List<ReviewGetDto>>> GetReviewsForMovie(Guid movieId)
        {

            var result = await _reviewService.GetReviewsByMovieIdAsync(movieId);
            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ReviewGetDto>> GetById(Guid id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ReviewGetDto>> Create([FromBody] ReviewCreateDto createDto)
        {
            var createdReview = await _reviewService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdReview.Id }, createdReview);
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ReviewGetDto>> Update(Guid id, [FromBody] ReviewUpdateDto updateDto)
        {
            var updatedReview = await _reviewService.UpdateAsync(id, updateDto);
            if (updatedReview == null)
                return NotFound();

            return Ok(updatedReview);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _reviewService.DeleteAsync(id);
            return NoContent();
        }
    }
}
