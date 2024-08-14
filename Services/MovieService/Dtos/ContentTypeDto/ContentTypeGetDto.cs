using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.ContentTypeDto
{
	public class ContentTypeGetDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
