using MovieReviewApp.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieService.Models
{
	public class ContentType : IEntity
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
        [MinLength(1, ErrorMessage = "Name cannot be empty.")]
        public string Name { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
    }
}
