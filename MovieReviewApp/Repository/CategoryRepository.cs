using MovieReviewApp.CommonModels.Interfaces;
using MovieService.Models;

namespace MovieService.Repository
{
	public class CategoryRepository : IBaseRepository<ContentType>
	{
		public Task<ContentType> CreateAsync(ContentType entity)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<ContentType>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ContentType> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ContentType> UpdateAsync(ContentType entity)
		{
			throw new NotImplementedException();
		}
	}
}