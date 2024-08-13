using MovieService.Dtos.MovieDto;

namespace MovieService.Service
{
	public interface IMovieService
	{
		Task<IEnumerable<MovieGetDto>> GetAllWithDetailsAsync();
		Task<MovieGetDto> GetByIdWithDetailsAsync(Guid id);
		Task<MovieGetDto> CreateAsync(MovieCreateDto createDto);
		Task<MovieGetDto> UpdateAsync(Guid id, MovieUpdateDto updateDto);
		Task DeleteAsync(Guid id);
	}
}
