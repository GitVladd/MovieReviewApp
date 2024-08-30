using MovieReviewApp.Common.Enums;
using MovieService.Dtos.MovieDto;
using MovieService.Models;
using System.Linq.Expressions;

namespace MovieService.Service
{
	public interface IMovieService
	{
		Task<IEnumerable<MovieGetDto>> GetAsync(
			Expression<Func<Movie, bool>> predicate = null,
			IEnumerable<Expression<Func<Movie, object>>> include = null,
			int take = int.MaxValue, int skip = 0,
			IEnumerable<Expression<Func<Movie, object>>> sortBy = null,
			SortDirection sortDirection = SortDirection.Ascending,
			CancellationToken cancellationToken = default);
		Task<IEnumerable<MovieGetDto>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
		Task<MovieGetDto> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
		Task<MovieGetDto> CreateAsync(MovieCreateDto createDto);
		Task<MovieGetDto> UpdateAsync(Guid id, MovieUpdateDto updateDto);
		Task DeleteAsync(Guid id);
	}
}
