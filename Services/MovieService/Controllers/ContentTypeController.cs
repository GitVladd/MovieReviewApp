using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieReviewApp.Common.Repository;
using MovieService.Dtos.ContentTypeDto;
using MovieService.Models;
using System.Reflection;

namespace MovieService.Controllers
{

	[ApiController]
	[Route("api/[controller]")]
	public class ContentTypeController : Controller
	{
		private readonly ILogger<ContentTypeController> _logger;
		private readonly IBaseRepository<ContentType> _repository;
		private readonly IMapper _mapper;
		public ContentTypeController(ILogger<ContentTypeController> logger, IBaseRepository<ContentType> repository, IMapper mapper)
		{
			_logger = logger;
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<List<ContentTypeGetDto>>> GetAll()
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entities = await _repository.GetAsync();
				var entitiesGetDtos = _mapper.Map<IEnumerable<ContentTypeGetDto>>(entities);
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
				var entity = await _repository.GetByIdAsync(id);

				if (entity == null)
					return NotFound();

				return Ok(_mapper.Map<ContentTypeGetDto>(entity));
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
		public async Task<ActionResult<ContentTypeGetDto>> Create([FromBody] ContentTypeCreateDto createDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var entity = _mapper.Map<ContentType>(createDto);

				_repository.Create(entity);
				await _repository.SaveAsync();

				var entityGetDto = _mapper.Map<ContentTypeGetDto>(entity);

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
