using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApp.Common.Repository;
using MovieService.Dtos.MovieDto;
using MovieService.Models;
using MovieService.Repository;
using System.Linq.Expressions;
using System.Reflection;

namespace MovieService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MovieController : ControllerBase
	{
		private readonly ILogger<MovieController> _logger;
		private readonly IBaseRepository<Movie> _repository;
		private readonly IBaseRepository<Category> _categoryRepository;
		private readonly IBaseRepository<ContentType> _contentTypeRepository;
		private readonly IMapper _mapper;
		public MovieController(
			ILogger<MovieController> logger,
			IBaseRepository<Movie> repository,
			IBaseRepository<Category> categoryRepository,
			IBaseRepository<ContentType> contentTypeRepository,
			IMapper mapper)
		{
			_logger = logger;
			_repository = repository;
			_categoryRepository = categoryRepository;
			_contentTypeRepository = contentTypeRepository;
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
				var className = this.GetType().Name;
				var methodName = MethodBase.GetCurrentMethod()?.Name;
				_logger.LogError(ex, $"Error in {className}.{methodName}");
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
				var className = this.GetType().Name;
				var methodName = MethodBase.GetCurrentMethod()?.Name;
				_logger.LogError(ex, $"Error in {className}.{methodName}");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the movie.");
			}
		}

		[HttpPost]
		public async Task<ActionResult<MovieGetDto>> Create([FromBody] MovieCreateDto createDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var contentType = await _contentTypeRepository.GetByIdAsync(createDto.ContentTypeId);

				if (contentType == null)
					return NotFound($"There is no content type with the following id: {createDto.ContentTypeId}");


				var categories = await _categoryRepository.GetAsync(c => createDto.CategoryIds.Contains(c.Id));
				// Check if the number of found categories matches the number of requested categories
				if (categories.Count != createDto.CategoryIds.Count)
				{
					var foundCategoryIds = categories.Select(c => c.Id).ToList();
					var missingCategoryIds = createDto.CategoryIds.Except(foundCategoryIds).ToList();
					if (missingCategoryIds.Any())
					{
						return NotFound($"The following category IDs were not found: {string.Join(", ", missingCategoryIds)}");
					}
				}


				var entity = _mapper.Map<Movie>(createDto);
				entity.ContentType = contentType;
				entity.Categories = categories;

				_repository.Create(entity);
				await _repository.SaveAsync();

				var entityGetDto = _mapper.Map<MovieGetDto>(entity);

				return CreatedAtAction(nameof(GetById), new { id = entityGetDto.Id }, entityGetDto);
			}
			catch (Exception ex)
			{
				var className = this.GetType().Name;
				var methodName = MethodBase.GetCurrentMethod()?.Name;
				_logger.LogError(ex, $"Error in {className}.{methodName}");
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

				if (entity.ContentType.Id != updateDto.ContentTypeId)
				{
					var contentType = await _contentTypeRepository.GetByIdAsync(updateDto.ContentTypeId);

					if (contentType == null)
						return NotFound($"Content type with id {updateDto.ContentTypeId} not found.");

					entity.ContentType = contentType;
				}

				var categories = await _categoryRepository.GetAsync(c => updateDto.CategoryIds.Contains(c.Id));
				// Check if the number of found categories matches the number of requested categories
				var missingCategoryIds = updateDto.CategoryIds
					.Where(categoryId => !categories.Select(c => c.Id).Contains(categoryId))
					.ToList();
				if (missingCategoryIds.Any())
				{
					return NotFound($"The following category IDs were not found: {string.Join(", ", missingCategoryIds)}");
				}


				entity = _mapper.Map(updateDto, entity);

				_repository.Update(entity);
				await _repository.SaveAsync();

				var movieGetDto = _mapper.Map<MovieGetDto>(entity);

				return Ok(movieGetDto);
			}
			catch (Exception ex)	
			{
				var className = this.GetType().Name;
				var methodName = MethodBase.GetCurrentMethod()?.Name;
				_logger.LogError(ex, $"Error in {className}.{methodName}");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the movie.");
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

				return NoContent();
			}
			catch (Exception ex)
			{
				var className = this.GetType().Name;
				var methodName = MethodBase.GetCurrentMethod()?.Name;
				_logger.LogError(ex, $"Error in {className}.{methodName}");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the movie.");
			}
		}
	}
}
