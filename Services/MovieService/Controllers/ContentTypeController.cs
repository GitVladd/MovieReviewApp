using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Dtos.ContentTypeDto;
using MovieService.Service;
using System.Reflection;

namespace MovieService.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class ContentTypeController : Controller
	{
		private readonly ILogger<ContentTypeController> _logger;
		private readonly IContentTypeService _service;
		public ContentTypeController(
			ILogger<ContentTypeController> logger, 
			IContentTypeService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<List<ContentTypeGetDto>>> GetAll()
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
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching content types.");
			}
		}

		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<List<ContentTypeGetDto>>> GetById([FromRoute] Guid id)
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
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the content type.");
			}

		}


		[HttpPost]
		[Authorize(Roles = "Admin, Moderator")]

		public async Task<ActionResult<ContentTypeGetDto>> Create([FromBody] ContentTypeCreateDto createDto)
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
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the contnt type.");
			}

		}
	}
}
