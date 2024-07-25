using MovieReviewApp.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Models
{
	public class Category : IEntity
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

		public IEnumerable<Movie> Movies { get; set; }
	}
}
