using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos
{
	public class UserRegisterDto
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(6)]
		public string Password { get; set; }
	}
}