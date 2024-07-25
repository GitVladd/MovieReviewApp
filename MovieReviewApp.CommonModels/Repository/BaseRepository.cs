using Microsoft.EntityFrameworkCore;
using MovieReviewApp.Data.Entities;
using MovieReviewApp.Data.Enums;
using MovieReviewApp.Data.Repository;
using System.Linq.Expressions;

namespace MovieReviewApp.CommonModels.BaseClass
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class,IEntity
	{
		protected readonly DbContext _context;

		public BaseRepository(DbContext context)
		{
			_context = context;
		}

		public async Task<T> GetByIdAsync(Guid id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public async Task<T> CreateAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<T> UpdateAsync(T entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<bool> DeleteByIdAsync(Guid id)
		{
			var entity = await _context.Set<T>().FindAsync(id);
			if (entity == null) return false;

			_context.Set<T>().Remove(entity);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null, int take = int.MaxValue, int skip = 0, IEnumerable<Expression<Func<T, object>>> sortBy = null, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public async Task SaveAsync(CancellationToken cancellationToken = default)
		{
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
