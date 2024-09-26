using MovieService.Dtos.CategoryDto;
using MovieService.Enums;
using MovieService.Models;
using System.Linq.Expressions;

namespace MovieService.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryGetDto>> GetAsync(
            Expression<Func<Category, bool>> predicate = null,
            IEnumerable<Expression<Func<Category, object>>> include = null,
            int take = int.MaxValue, int skip = 0,
            IEnumerable<Expression<Func<Category, object>>> sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            CancellationToken cancellationToken = default);
        Task<CategoryGetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CategoryGetDto> CreateAsync(CategoryCreateDto createDto);
    }
}
