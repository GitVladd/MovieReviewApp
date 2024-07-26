using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApp.Common.Repository;
using MovieService.Dtos.CategoryDto;
using MovieService.Models;
using System.Reflection;

namespace MovieService.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ILogger<CategoryController> _logger;
		private readonly IBaseRepository<Category> _repository;
		private readonly IMapper _mapper;
		public CategoryController(ILogger<CategoryController> logger, IBaseRepository<Category> repository, IMapper mapper)
		{
			_logger = logger;
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<List<CategoryGetDto>>> GetAll()
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entities = await _repository.GetAsync();
				var entitiesGetDtos = _mapper.Map<IEnumerable<CategoryGetDto>>(entities);
				return Ok(entitiesGetDtos);
			}
			catch (Exception ex)
			{
				var className = this.GetType().Name;
				var methodName = MethodBase.GetCurrentMethod()?.Name;
				_logger.LogError(ex, $"Error in {className}.{methodName}");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching categories.");
			}
		}

		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<List<CategoryGetDto>>> GetById([FromRoute] Guid id)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entity = await _repository.GetByIdAsync(id);

				if (entity == null)
					return NotFound();

				return Ok(_mapper.Map<CategoryGetDto>(entity));
			}
			catch (Exception ex)
			{
				var className = this.GetType().Name;
				var methodName = MethodBase.GetCurrentMethod()?.Name;
				_logger.LogError(ex, $"Error in {className}.{methodName}");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the category.");
			}
		}

		[HttpPost]
		public async Task<ActionResult<CategoryGetDto>> Create([FromBody] CategoryCreateDto createDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entity = _mapper.Map<Category>(createDto);

				_repository.Create(entity);
				await _repository.SaveAsync();

				var entityGetDto = _mapper.Map<CategoryGetDto>(entity);

				return CreatedAtAction(nameof(GetById), new { id = entityGetDto.Id }, entityGetDto);
			}
			catch (Exception ex)
			{
				var className = this.GetType().Name;
				var methodName = MethodBase.GetCurrentMethod()?.Name;
				_logger.LogError(ex, $"Error in {className}.{methodName}");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the category.");
			}

		}
	}
}
