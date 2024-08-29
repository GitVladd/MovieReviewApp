using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Dtos.MovieDto;
using MovieService.Service;

namespace MovieService.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(
            IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet]
        public async Task<ActionResult<List<MovieGetDto>>> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _movieService.GetAllWithDetailsAsync();
            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<MovieGetDto>> GetById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var entity = await _movieService.GetByIdWithDetailsAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]

        public async Task<ActionResult<MovieGetDto>> Create([FromBody] MovieCreateDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdEntity = await _movieService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Admin, Moderator")]

        public async Task<ActionResult<MovieGetDto>> Update([FromRoute] Guid id, [FromBody] MovieUpdateDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var updatedEntity = await _movieService.UpdateAsync(id, updateDto);
            if (updatedEntity == null)
                return NotFound();

            return Ok(updatedEntity);
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _movieService.DeleteAsync(id);
            return NoContent();
        }
    }
}
