using Microsoft.AspNetCore.Mvc;
using MovieService.Dtos.CategoryDto;
using MovieService.Models;
using MovieService.Service;
using System.Reflection;

namespace MovieService.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly ILogger<CategoryController> _logger;
		private readonly ICategoryService _service;
		public CategoryController(
			ILogger<CategoryController> logger, 
			ICategoryService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<List<CategoryGetDto>>> GetAll()
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entitiesGetDtos = await _service.GetAsync();
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
				var entityGetDto = await _service.GetByIdAsync(id);

				if (entityGetDto == null)
					return NotFound();

				return Ok(entityGetDto);
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
				var entityGetDto = await _service.CreateAsync(createDto);

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
