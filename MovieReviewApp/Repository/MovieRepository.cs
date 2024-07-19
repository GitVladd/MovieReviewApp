using MovieReviewApp.CommonModels.Interfaces;
using MovieService.Models;

namespace MovieService.Repository
{
	public class MovieRepository : IBaseRepository<Movie>
	{
		public Task<Movie> CreateAsync(Movie entity)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Movie>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Movie> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Movie> UpdateAsync(Movie entity)
		{
			throw new NotImplementedException();
		}
	}
}
