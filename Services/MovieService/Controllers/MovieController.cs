using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApp.Common.Repository;
using MovieService.Dtos.MovieDto;
using MovieService.Models;

namespace MovieService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MovieController : ControllerBase
	{
		private readonly ILogger<MovieController> _logger;
		private readonly IBaseRepository<Movie> _repository;
		private readonly IMapper _mapper;
		public MovieController(ILogger<MovieController> logger, IBaseRepository<Movie> repository, IMapper mapper)
		{
			_logger = logger;
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<List<MovieGetDto>>> GetAll()
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entities = await _repository.GetAsync();
				var entitiesGetDtos = _mapper.Map<IEnumerable<MovieGetDto>>(entities);
				return Ok(entitiesGetDtos);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error: Movie Controller GetAll()");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching movies.");
			}
		}


		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<List<MovieGetDto>>> GetById([FromRoute] Guid id)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entity = await _repository.GetByIdAsync(id);

				if (entity == null)
					return NotFound();

				return Ok(_mapper.Map<MovieGetDto>(entity));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error: Movie Controller GetById()");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching movies.");
			}
		}

		[HttpPost]
		public async Task<ActionResult<MovieGetDto>> Create([FromBody] MovieCreateDto createDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			try
			{
				var entity = _mapper.Map<Movie>(createDto);

				_repository.Create(entity);
				await _repository.SaveAsync();

				var entityGetDto = _mapper.Map<MovieGetDto>(entity);

				return CreatedAtAction(nameof(GetById), new { id = entityGetDto.Id }, entityGetDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error: Movie Controller Create()");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the movie.");
			}
		}

		[HttpPut("{id:Guid}")]
		public async Task<ActionResult<MovieGetDto>> Update(Guid id, [FromBody] MovieUpdateDto updateDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			try
			{
				var entity = await _repository.GetByIdAsync(id);

				if (entity == null)
					return NotFound();

				_repository.Update(entity);
				await _repository.SaveAsync();

				var movieGetDto = _mapper.Map<MovieGetDto>(entity);

				return CreatedAtAction(nameof(GetById), new { id = movieGetDto.Id }, movieGetDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error: Movie Controller Update()");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the movie.");
			}
		}
		[HttpDelete("{id:Guid}")]
		public async Task<ActionResult> Delete(Guid id)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			try
			{
				var entity = await _repository.GetByIdAsync(id);

				if (entity == null)
					return NotFound();

				_repository.Delete(entity);
				await _repository.SaveAsync();

				var movieGetDto = _mapper.Map<MovieGetDto>(entity);

				return CreatedAtAction(nameof(GetById), new { id = movieGetDto.Id }, movieGetDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error: Movie Controller Delete()");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the movie.");
			}
		}
	}
}
