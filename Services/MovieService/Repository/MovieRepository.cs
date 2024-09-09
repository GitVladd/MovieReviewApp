using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Common.Enums;
using MovieReviewApp.Common.Repository;
using MovieService.Data;
using MovieService.Models;
using System.Linq.Expressions;

namespace MovieService.Repository
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
	{
		public MovieRepository(ApplicationDbContext context) : base(context)
		{

		}
		public async Task<List<Movie>> GetAllWithDetailsAsync(
			Expression<Func<Movie, bool>> predicate = null,
			IEnumerable<Expression<Func<Movie, object>>> include = null,
			int take = int.MaxValue, 
			int skip = 0, 
			IEnumerable<Expression<Func<Movie, object>>> sortBy = null, 
			SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
		{
			var query = _context.Set<Movie>().AsQueryable();

			if (predicate != null) 
				query = query.Where(predicate);

			if (sortBy != null && sortBy.Any())
			{
				var orderedQuery = sortDirection == SortDirection.Ascending
					? query.OrderBy(sortBy.First())
					: query.OrderByDescending(sortBy.First());

				foreach (var sortExpression in sortBy.Skip(1))
				{
					orderedQuery = sortDirection == SortDirection.Ascending
						? orderedQuery.ThenBy(sortExpression)
						: orderedQuery.ThenByDescending(sortExpression);
				}

				query = orderedQuery;
			}

			query = query.Skip(skip).Take(take);

			return await query.
				Include(m => m.Categories).
				Include(m => m.ContentType).
				ToListAsync(cancellationToken);
		}

		public async Task<Movie> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Movie>()
						 .Include(m => m.Categories)
						 .Include(m => m.ContentType)
						 .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

		}
	}
}	
