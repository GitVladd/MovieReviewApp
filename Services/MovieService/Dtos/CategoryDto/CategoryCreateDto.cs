using MovieService.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.CategoryDto
{
	public class CategoryCreateDto
	{
		[Required]
		public string Name { get; set; }
	}
}
