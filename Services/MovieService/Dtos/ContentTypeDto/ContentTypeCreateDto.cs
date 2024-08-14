using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.ContentTypeDto
{
	public class ContentTypeCreateDto
	{
		[Required]
		public string Name { get; set; }
	}
}
