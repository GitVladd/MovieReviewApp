using AutoMapper;
using MovieReviewApp.Common.Enums;
using MovieReviewApp.Common.Repository;
using MovieService.Controllers;
using MovieService.Dtos.MovieDto;
using MovieService.Models;
using MovieService.Repository;
using System.Linq.Expressions;

namespace MovieService.Service
{
	public class MovieService : IMovieService
	{
		private readonly ILogger<MovieController> _logger;

		private readonly IMovieRepository _repository;

		private readonly ICategoryService _categoryService;
		private readonly IContentTypeService _contentTypeService;

		private readonly IMapper _mapper;

		public MovieService(
			ILogger<MovieController> logger,

			IMovieRepository movieRepository,

			ICategoryService categoryService,
			IContentTypeService contentTypeService,

			IMapper mapper)
		{
			_logger = logger;

			_repository = movieRepository;

			_categoryService = categoryService;
			_contentTypeService = contentTypeService;

			_mapper = mapper;
		}

		public async Task<IEnumerable<MovieGetDto>> GetAsync(
			Expression<Func<Movie, bool>> predicate = null,
			IEnumerable<Expression<Func<Movie, object>>> include = null,
			int take = int.MaxValue, int skip = 0,
			IEnumerable<Expression<Func<Movie, object>>> sortBy = null,
			SortDirection sortDirection = SortDirection.Ascending,
			CancellationToken cancellationToken = default)
		{
			var entities = await _repository.GetAsync(predicate, include, take, skip, sortBy,sortDirection, cancellationToken) ;
			return _mapper.Map<IEnumerable<MovieGetDto>>(entities);
		}

		public async Task<IEnumerable<MovieGetDto>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
		{
			var includeExpressions = new List<Expression<Func<Movie, object>>>
			{
				m => m.Categories,
				m => m.ContentType
			};

			var entities = await _repository.GetAsync(
				include: includeExpressions
			);
			var entitiesGetDtos = _mapper.Map<IEnumerable<MovieGetDto>>(entities);
			return entitiesGetDtos;
		}
		public async Task<MovieGetDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
		{
			Expression<Func<Movie, bool>> predicate = m => m.Id == id;

			var includeExpressions = new List<Expression<Func<Movie, object>>>
			{
				m => m.Categories,
				m => m.ContentType
			};

			var entities = await _repository.GetAsync(
				predicate: predicate,
				include: includeExpressions
			);

			var entity = entities.FirstOrDefault();

			if (entity == null)
			{
				return null; // Return null to indicate not found
			}
			var entityGetDto = _mapper.Map<MovieGetDto>(entity);
			return entityGetDto;
		}
		public async Task<MovieGetDto> CreateAsync(MovieCreateDto createDto)
		{
			var contentType = await _contentTypeService.GetByIdAsync(createDto.ContentTypeId);
			if (contentType == null)
				throw new ArgumentException($"There is no content type with the id: {createDto.ContentTypeId}");

			var categories = await _categoryService.GetAsync( predicate: c => createDto.CategoryIds.Contains(c.Id));
			if (categories.Count() != createDto.CategoryIds.Count())
			{
				var foundCategoryIds = categories.Select(c => c.Id).ToList();
				var missingCategoryIds = createDto.CategoryIds.Except(foundCategoryIds).ToList();
				throw new ArgumentException($"The following category IDs were not found: {string.Join(", ", missingCategoryIds)}");
			}

			var entity = _mapper.Map<Movie>(createDto);
			entity.ContentType = _mapper.Map<ContentType>(contentType);
			entity.Categories = _mapper.Map<IEnumerable<Category>>(categories);

			_repository.Create(entity);
			await _repository.SaveAsync();

			var entityGetDto = _mapper.Map<MovieGetDto>(entity);
			return entityGetDto;
		}
		public async Task<MovieGetDto> UpdateAsync(Guid id, MovieUpdateDto updateDto)
		{
			var entity = await _repository.GetByIdWithDetailsAsync(id);
			if (entity == null) 
				return null; // Return null to indicate not found

			if (entity.ContentType.Id != updateDto.ContentTypeId)
			{
				var contentType = await _contentTypeService.GetByIdAsync(updateDto.ContentTypeId);
				if (contentType == null)
				{
					throw new ArgumentException($"Content type with id {updateDto.ContentTypeId} not found.");
				}
				entity.ContentType = _mapper.Map<ContentType>(contentType);
			}

			var categories = await _categoryService.GetAsync(c => updateDto.CategoryIds.Contains(c.Id));
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
