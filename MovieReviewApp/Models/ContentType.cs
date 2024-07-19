using System.ComponentModel.DataAnnotations;

namespace MovieService.Models
{
	public class ContentType
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
	}
}
