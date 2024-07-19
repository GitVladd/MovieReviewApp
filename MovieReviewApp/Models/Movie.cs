using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieService.Models
{
	public class Movie
	{
		[Key]
		int Id { get; set; }

		[Required]
		[StringLength(32)]
		string Title { get; set; } = string.Empty;

		[StringLength(1024)]
		string Description { get; set; } = string.Empty;

		[Required]
		List<ContentType> Categories { get; set; } = new List<ContentType>();

		[Required]
		ContentType ContentType { get; set; }

		[Column(TypeName = "Date")]
		DateTime? ReleaseDate { get; set; }

		[Required]
		[StringLength(128)]
		string Creators { get; set; }

		[StringLength(512)]
		string Actors { get; set; } = string.Empty;


	}
}
