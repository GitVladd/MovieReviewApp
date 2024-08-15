using AutoMapper;
using MovieReviewApp.Common.Enums;
using MovieReviewApp.Common.Repository;
using MovieService.Dtos.CategoryDto;
using MovieService.Models;
using System.Globalization;
using System;
using System.Linq.Expressions;
using System.Threading;

namespace MovieService.Service
{
	public class CategoryService : ICategoryService
	{
		private readonly ILogger<CategoryService> _logger;
		private readonly IBaseRepository<Category> _repository;
		private readonly IMapper _mapper;

		public CategoryService(ILogger<CategoryService> logger, IBaseRepository<Category> repository, IMapper mapper)
		{
			_logger = logger;
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<CategoryGetDto>> GetAsync(
			Expression<Func<Category, bool>> predicate = null,
			IEnumerable<Expression<Func<Category, object>>> include = null,
			int take = int.MaxValue, int skip = 0,
			IEnumerable<Expression<Func<Category, object>>> sortBy = null,
			SortDirection sortDirection = SortDirection.Ascending,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var entities = await _repository.GetAsync(predicate, include, take, skip, sortBy, sortDirection, cancellationToken);
				var result = _mapper.Map<IEnumerable<CategoryGetDto>>(entities);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching categories.");
				throw;
			}
		}

		public async Task<CategoryGetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			try
			{
				Expression<Func<Category, bool>> predicate = m => m.Id == id;

				var entities = await _repository.GetAsync(predicate: predicate);

				var entity = entities.FirstOrDefault();

				if (entity == null)
				{
					return null;
				}

				var result = _mapper.Map<CategoryGetDto>(entity);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching category with ID {CategoryId}.", id);
				throw;
			}
		}

		public async Task<CategoryGetDto> CreateAsync(CategoryCreateDto createDto)
		{
			try
			{
				var entity = _mapper.Map<Category>(createDto);
				_repository.Create(entity);
				await _repository.SaveAsync();
				var result = _mapper.Map<CategoryGetDto>(entity);
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while creating a new category.");
				throw;
			}
		}
	}
}
