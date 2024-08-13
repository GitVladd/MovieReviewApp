using Microsoft.AspNetCore.Mvc;
using MovieService.Dtos.MovieDto;
using MovieService.Models;
using MovieService.Service;
using System.Reflection;

namespace MovieService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MovieController : ControllerBase
	{
		private readonly ILogger<MovieController> _logger;
		private readonly IMovieService _movieService;
		public MovieController(
			ILogger<MovieController> logger,
			IMovieService movieService)
		{
			_logger = logger;
			_movieService = movieService;
		}
		[HttpGet]
		public async Task<ActionResult<List<MovieGetDto>>> GetAll()
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var result = await _movieService.GetAllWithDetailsAsync();
				return Ok(result);
			}
			catch (Exception ex)
			{
				return HandleError(ex, nameof(GetAll));
			}
		}

		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<MovieGetDto>> GetById([FromRoute] Guid id)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entity = await _movieService.GetByIdWithDetailsAsync(id);
				if (entity == null)
					return NotFound();

				return Ok(entity);
			}
			catch (Exception ex)
			{
				return HandleError(ex, nameof(GetById));
			}
		}

		[HttpPost]
		public async Task<ActionResult<MovieGetDto>> Create([FromBody] MovieCreateDto createDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var createdEntity = await _movieService.CreateAsync(createDto);
				return CreatedAtAction(nameof(GetById), new { id = createdEntity.Id }, createdEntity);
			}
			catch (Exception ex)
			{
				return HandleError(ex, nameof(Create));
			}
		}

		[HttpPut("{id:Guid}")]
		public async Task<ActionResult<MovieGetDto>> Update([FromRoute] Guid id, [FromBody] MovieUpdateDto updateDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var updatedEntity = await _movieService.UpdateAsync(id, updateDto);
				if (updatedEntity == null)
					return NotFound();

				return Ok(updatedEntity);
			}
			catch (Exception ex)
			{
				return HandleError(ex, nameof(Update));
			}
		}

		[HttpDelete("{id:Guid}")]
		public async Task<ActionResult> Delete([FromRoute] Guid id)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				await _movieService.DeleteAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return HandleError(ex, nameof(Delete));
			}
		}

		private ActionResult HandleError(Exception ex, string methodName)
		{
			var className = GetType().Name;
			_logger.LogError(ex, $"Error in {className}.{methodName}");
			return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request in {className}.{methodName}.");
		}
	}
}
