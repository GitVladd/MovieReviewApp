//using Microsoft.EntityFrameworkCore;
//using MovieReviewApp.Common.Entities;
//using MovieReviewApp.Common.Enums;
//using MovieReviewApp.Common.Repository;
//using MovieReviewApp.Data;
//using System.Linq.Expressions;

//namespace MovieService.Repository
//{
//	public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
//	{
//		protected readonly ApplicationDbContext _context;

//		public BaseRepository(ApplicationDbContext context)
//		{
//			_context = context;
//		}

//		public async Task<List<T>> GetAsync(
//			Expression<Func<T, bool>> predicate = null,
//			IEnumerable<Expression<Func<T, object>>> include = null,
//			int take = int.MaxValue, int skip = 0,
//			IEnumerable<Expression<Func<T, object>>> sortBy = null,
//			SortDirection sortDirection = SortDirection.Ascending,
//			CancellationToken cancellationToken = default
//			)
//		{
//			var query = _context.Set<T>().AsQueryable();

//			if (include != null && include.Any())
//			{
//				query = include.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
//			}

//			if (predicate != null)
//				query = query.Where(predicate);

//			if (sortBy != null && sortBy.Any())
//			{
//				var orderedQuery = sortDirection == SortDirection.Ascending
//					? query.OrderBy(sortBy.First())
//					: query.OrderByDescending(sortBy.First());

//				foreach (var sortExpression in sortBy.Skip(1))
//				{
//					orderedQuery = sortDirection == SortDirection.Ascending
//						? orderedQuery.ThenBy(sortExpression)
//						: orderedQuery.ThenByDescending(sortExpression);
//				}

//				query = orderedQuery;
//			}
//			query = query.Skip(skip).Take(take);

//			return await query.ToListAsync(cancellationToken);
//		}

//		public void Create(T entity)
//		{
//			_context.Set<T>().Add(entity);
//		}

//		public void Update(T entity)
//		{
//			_context.Set<T>().Update(entity);
//		}

//		public void Delete(T entity)
//		{
//			_context.Set<T>().Remove(entity);
//		}

//		public async Task SaveAsync(CancellationToken cancellationToken = default)
//		{
//			await _context.SaveChangesAsync(cancellationToken);
//		}
//	}
//}
