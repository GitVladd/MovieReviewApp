using MovieReviewApp.Common.Entities;
using MovieReviewApp.Common.Enums;
using System.Linq.Expressions;

namespace MovieReviewApp.Common.Repository
{
    public interface IBaseRepository<T> where T : IEntity
	{
		Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null, int take = int.MaxValue, int skip = 0, IEnumerable<Expression<Func<T, object>>> sortBy = null, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);
		Task<T> GetByIdAsync(Guid id);
		Task<T> CreateAsync(T entity);
		Task<T> UpdateAsync(T entity);
		Task<bool> DeleteByIdAsync(Guid id);
		Task SaveAsync(CancellationToken cancellationToken = default);
	}
}
