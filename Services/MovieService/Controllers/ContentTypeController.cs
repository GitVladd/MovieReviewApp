using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Dtos.ContentTypeDto;
using MovieService.Service;

namespace MovieService.Controllers
{

    [ApiController]
    [Route("api/contenttypes")]
    public class ContentTypeController : Controller
    {
        private readonly IContentTypeService _service;
        public ContentTypeController(
            IContentTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContentTypeGetDto>>> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entitiesGetDtos = await _service.GetAsync();
            return Ok(entitiesGetDtos);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<List<ContentTypeGetDto>>> GetById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entityGetDto = await _service.GetByIdAsync(id);

            if (entityGetDto == null)
                return NotFound();

            return Ok(entityGetDto);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult<ContentTypeGetDto>> Create([FromBody] ContentTypeCreateDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entityGetDto = await _service.CreateAsync(createDto);

            return CreatedAtAction(nameof(GetById), new { id = entityGetDto.Id }, entityGetDto);

        }
    }
}
