using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.CategoryDto
{
	public class CategoryGetDto
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
