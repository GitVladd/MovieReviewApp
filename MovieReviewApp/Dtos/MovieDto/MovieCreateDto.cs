using MovieService.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MovieReviewApp.Common.Attributes;

namespace MovieService.Dtos.MovieDto
{
	public class MovieCreateDto
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
