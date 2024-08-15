using MovieReviewApp.Common.Enums;
using MovieService.Dtos.ContentTypeDto;
using MovieService.Models;
using System.Linq.Expressions;

namespace MovieService.Service
{
	public interface IContentTypeService
	{
		Task<IEnumerable<ContentTypeGetDto>> GetAsync(
			Expression<Func<ContentType, bool>> predicate = null,
			IEnumerable<Expression<Func<ContentType, object>>> include = null,
			int take = int.MaxValue, int skip = 0,
			IEnumerable<Expression<Func<ContentType, object>>> sortBy = null,
			SortDirection sortDirection = SortDirection.Ascending,
			CancellationToken cancellationToken = default);
		Task<ContentTypeGetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<ContentTypeGetDto> CreateAsync(ContentTypeCreateDto createDto);
	}
}
