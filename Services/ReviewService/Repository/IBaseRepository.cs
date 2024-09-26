using ReviewService.Entities;
using ReviewService.Enums;
using System.Linq.Expressions;

namespace ReviewService.Repository
{
    public interface IBaseRepository<T> where T : IEntity
    {
        Task<List<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            IEnumerable<Expression<Func<T, object>>> include = null,
            int take = int.MaxValue,
            int skip = 0,
            IEnumerable<Expression<Func<T, object>>> sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            CancellationToken cancellationToken = default);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
