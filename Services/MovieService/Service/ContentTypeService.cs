using AutoMapper;
using MovieReviewApp.Common.Enums;
using MovieReviewApp.Common.Repository;
using MovieService.Dtos.ContentTypeDto;
using MovieService.Models;
using System.Linq.Expressions;

namespace MovieService.Service
{
	public class ContentTypeService : IContentTypeService
	{
		private readonly ILogger<ContentTypeService> _logger;
		private readonly IBaseRepository<ContentType> _repository;
		private readonly IMapper _mapper;

		public ContentTypeService(ILogger<ContentTypeService> logger, IBaseRepository<ContentType> repository, IMapper mapper)
		{
			_logger = logger;
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ContentTypeGetDto>> GetAsync(
			Expression<Func<ContentType, bool>> predicate = null,
			IEnumerable<Expression<Func<ContentType, object>>> include = null,
			int take = int.MaxValue, int skip = 0,
			IEnumerable<Expression<Func<ContentType, object>>> sortBy = null,
			SortDirection sortDirection = SortDirection.Ascending,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var entities = await _repository.GetAsync(predicate,include,take,skip,sortBy,sortDirection,cancellationToken);
				var result = _mapper.Map<IEnumerable<ContentTypeGetDto>>(entities);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching content types.");
				throw; // Re-throw the exception after logging it
			}
		}

		public async Task<ContentTypeGetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			try
			{
				Expression<Func<ContentType, bool>> predicate = m => m.Id == id;

				var entities = await _repository.GetAsync(predicate : predicate);

				var entity = entities.FirstOrDefault();

				if (entity == null)
				{
					return null;
				}

				var result = _mapper.Map<ContentTypeGetDto>(entity);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching content type with ID {ContentTypeId}.", id);
				throw;
			}
		}

		public async Task<ContentTypeGetDto> CreateAsync(ContentTypeCreateDto createDto)
		{
			try
			{
				var entity = _mapper.Map<ContentType>(createDto);
				_repository.Create(entity);
				await _repository.SaveAsync();
				var result = _mapper.Map<ContentTypeGetDto>(entity);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while creating a new content type.");
				throw;
			}
		}

	}
}