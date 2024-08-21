using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.ContentTypeDto
{
	public class ContentTypeCreateDto
	{
		[Required]
		[StringLength(64)]
		public string Name { get; set; }
	}
}
