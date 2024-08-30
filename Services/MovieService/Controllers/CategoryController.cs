using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieService.Dtos.CategoryDto;
using MovieService.Service;

namespace MovieService.Controllers
{

    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(
            ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryGetDto>>> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entitiesGetDtos = await _service.GetAsync();

            if (entitiesGetDtos == null || !entitiesGetDtos.Any())
                return NotFound();

            return Ok(entitiesGetDtos);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<List<CategoryGetDto>>> GetById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entityGetDto = await _service.GetByIdAsync(id);

            if (entityGetDto == null)
                return NotFound();

            return Ok(entityGetDto);
        }



        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult<CategoryGetDto>> Create([FromBody] CategoryCreateDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entityGetDto = await _service.CreateAsync(createDto);

            return CreatedAtAction(nameof(GetById), new { id = entityGetDto.Id }, entityGetDto);

        }
    }
}
