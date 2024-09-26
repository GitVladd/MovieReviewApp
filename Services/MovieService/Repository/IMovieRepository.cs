using MovieService.Enums;
using MovieService.Models;
using System.Linq.Expressions;

namespace MovieService.Repository
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        Task<List<Movie>> GetAllWithDetailsAsync(
            Expression<Func<Movie, bool>> predicate = null,
            IEnumerable<Expression<Func<Movie, object>>> include = null,
            int take = int.MaxValue, int skip = 0,
            IEnumerable<Expression<Func<Movie, object>>> sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            CancellationToken cancellationToken = default
            );
        Task<Movie> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
