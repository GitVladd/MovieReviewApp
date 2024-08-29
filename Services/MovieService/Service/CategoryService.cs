using AutoMapper;
using MovieReviewApp.Common.Enums;
using MovieReviewApp.Common.Repository;
using MovieService.Dtos.CategoryDto;
using MovieService.Models;
using System.Linq.Expressions;

namespace MovieService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category> _repository;
        private readonly IMapper _mapper;

        public CategoryService(IBaseRepository<Category> repository, IMapper mapper)
        {
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
            var entities = await _repository.GetAsync(predicate, include, take, skip, sortBy, sortDirection, cancellationToken);

            if (!entities.Any()) return null;

            var result = _mapper.Map<IEnumerable<CategoryGetDto>>(entities);
            return result;
        }

        public async Task<CategoryGetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
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

        public async Task<CategoryGetDto> CreateAsync(CategoryCreateDto createDto)
        {
            var entity = _mapper.Map<Category>(createDto);
            _repository.Create(entity);
            await _repository.SaveAsync();
            var result = _mapper.Map<CategoryGetDto>(entity);
            return result;
        }
    }
}
