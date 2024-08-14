using AutoMapper;
using MovieReviewApp.Common.Repository;
using MovieService.Controllers;
using MovieService.Dtos.MovieDto;
using MovieService.Models;
using MovieService.Repository;

namespace MovieService.Service
{
	public class MovieService : IMovieService
	{
		private readonly ILogger<MovieController> _logger;
		private readonly IMovieRepository _repository;
		private readonly IBaseRepository<Category> _categoryRepository;
		private readonly IBaseRepository<ContentType> _contentTypeRepository;
		private readonly IMapper _mapper;

		public MovieService(
			ILogger<MovieController> logger,
			IMovieRepository repository,
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

		public async Task<IEnumerable<MovieGetDto>> GetAllWithDetailsAsync()
		{
			var entities = await _repository.GetAllWithDetailsAsync();
			var entitiesGetDtos = _mapper.Map<IEnumerable<MovieGetDto>>(entities);
			return entitiesGetDtos;
		}
		public async Task<MovieGetDto> GetByIdWithDetailsAsync(Guid id)
		{
			var entity = await _repository.GetByIdWithDetailsAsync(id);
			if (entity == null)
			{
				return null; // Return null to indicate not found
			}
			var entityGetDto = _mapper.Map<MovieGetDto>(entity);
			return entityGetDto;
		}
		public async Task<MovieGetDto> CreateAsync(MovieCreateDto createDto)
		{
			var contentType = await _contentTypeRepository.GetByIdAsync(createDto.ContentTypeId);
			if (contentType == null)
			{
				throw new ArgumentException($"There is no content type with the id: {createDto.ContentTypeId}");
			}

			var categories = await _categoryRepository.GetAsync(c => createDto.CategoryIds.Contains(c.Id));
			if (categories.Count != createDto.CategoryIds.Count)
			{
				var foundCategoryIds = categories.Select(c => c.Id).ToList();
				var missingCategoryIds = createDto.CategoryIds.Except(foundCategoryIds).ToList();
				throw new ArgumentException($"The following category IDs were not found: {string.Join(", ", missingCategoryIds)}");
			}

			var entity = _mapper.Map<Movie>(createDto);
			entity.ContentType = contentType;
			entity.Categories = categories;

			_repository.Create(entity);
			await _repository.SaveAsync();

			var entityGetDto = _mapper.Map<MovieGetDto>(entity);
			return entityGetDto;
		}
		public async Task<MovieGetDto> UpdateAsync(Guid id, MovieUpdateDto updateDto)
		{
			var entity = await _repository.GetByIdWithDetailsAsync(id);
			if (entity == null)
			{
				return null; // Return null to indicate not found
			}

			if (entity.ContentType.Id != updateDto.ContentTypeId)
			{
				var contentType = await _contentTypeRepository.GetByIdAsync(updateDto.ContentTypeId);
				if (contentType == null)
				{
					throw new ArgumentException($"Content type with id {updateDto.ContentTypeId} not found.");
				}
				entity.ContentType = contentType;
			}

			var categories = await _categoryRepository.GetAsync(c => updateDto.CategoryIds.Contains(c.Id));
			var missingCategoryIds = updateDto.CategoryIds
				.Where(categoryId => !categories.Select(c => c.Id).Contains(categoryId))
				.ToList();
			if (missingCategoryIds.Any())
			{
				throw new ArgumentException($"The following category IDs were not found: {string.Join(", ", missingCategoryIds)}");
			}

			entity = _mapper.Map(updateDto, entity);

			_repository.Update(entity);
			await _repository.SaveAsync();

			var movieGetDto = _mapper.Map<MovieGetDto>(entity);
			return movieGetDto;
		}

		public async Task DeleteAsync(Guid id)
		{
			var entity = await _repository.GetByIdWithDetailsAsync(id);
			if (entity == null)
			{
				throw new ArgumentException($"Movie with id {id} not found.");
			}

			_repository.Delete(entity);
			await _repository.SaveAsync();
		}
	}
}
