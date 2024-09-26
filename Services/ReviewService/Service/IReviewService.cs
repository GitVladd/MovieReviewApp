using ReviewService.Dtos;
using ReviewService.Enums;
using ReviewService.Models;
using System.Linq.Expressions;

namespace ReviewService.Service
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewGetDto>> GetAsync(
            Expression<Func<Review, bool>> predicate = null,
            IEnumerable<Expression<Func<Review, object>>> include = null,
            int take = int.MaxValue, int skip = 0,
            IEnumerable<Expression<Func<Review, object>>> sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            CancellationToken cancellationToken = default);

        Task<ReviewGetDto> GetByIdAsync(Guid reviewId, CancellationToken cancellationToken = default);

        Task<List<ReviewGetDto>> GetReviewsByMovieIdAsync(Guid movieId);

        Task<ReviewGetDto> CreateAsync(ReviewCreateDto createDto, Guid UserId);

        Task<ReviewGetDto> UpdateAsync(Guid reviewId, ReviewUpdateDto updateDto, Guid UserId);

        Task DeleteAsync(Guid reviewId, Guid UserId);
    }
}
