using MovieService.Dtos.CategoryDto;
using MovieService.Dtos.ContentTypeDto;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieService.Dtos.MovieDto
{
	public class MovieGetDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public List<CategoryGetDto> Categories { get; set; }
		public ContentTypeGetDto ContentType { get; set; }
		public DateTime? ReleaseDate { get; set; }
	}
}
