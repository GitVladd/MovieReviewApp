using AutoMapper;
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
		public  MovieController(ILogger<MovieController> logger, IBaseRepository<Movie> repository, IMapper mapper)
		{
			_logger = logger;
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<List<MovieGetDto>>> GetAll()
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var results = await _repository.GetAsync();

			return Ok(_mapper.Map<IEnumerable<MovieGetDto>>(results));
		}
		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<List<MovieGetDto>>> GetById([FromRoute] Guid id)
		{
			//if (!ModelState.IsValid) return BadRequest(ModelState);

			//var comment = await _repository.GetByIdAsync(id);
			//return Ok(comment.ToCommentDto());
			throw new NotImplementedException();
		}

		[HttpPost]
		public async Task<ActionResult<MovieGetDto>> Create( [FromBody] MovieCreateDto creteDto)
		{
			throw new NotImplementedException();
		}

		[HttpPut("{id:Guid}")]
		public async Task<ActionResult<MovieGetDto>> Update(Guid id, [FromBody] MovieUpdateDto updateDto)
		{

			//if (!ModelState.IsValid) return BadRequest(ModelState);

			//var existingComment = await _repository.GetByIdAsync(id);
			//if (existingComment == null)
			//	return NotFound();

			//existingComment.Title = updateDto.Title;
			//existingComment.Content = updateDto.Content;

			//await _repository.UpdateAsync(existingComment);

			//return Ok(existingComment.ToCommentDto());
			throw new NotImplementedException();
		}
		[HttpDelete("{id:Guid}")]
		public async Task<ActionResult> Delete(Guid id)
		{
			//if (!ModelState.IsValid) return BadRequest(ModelState);

			//var success = await _repository.DeleteByIdAsync(id);
			//if (!success)
			//	return NotFound();
			//return NoContent();
			throw new NotImplementedException();
		}
	}
}
