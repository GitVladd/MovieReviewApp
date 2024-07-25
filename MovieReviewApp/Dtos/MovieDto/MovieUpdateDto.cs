using MovieReviewApp.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.MovieDto
{
	public class MovieUpdateDto
	{
		[Required]
		[StringLength(32)]
		public string Title { get; set; }

		[StringLength(1024)]
		public string Description { get; set; }

		[Required]
		[NonEmptyList(ErrorMessage = "CategoryIds cannot be empty")]
		public List<Guid> CategoryIds { get; set; }

		[Required]
		public Guid ContentTypeId { get; set; }
	}
}
