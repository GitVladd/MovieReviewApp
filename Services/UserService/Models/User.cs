using MovieReviewApp.Common.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
	public class User : IEntity
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		[Column(TypeName = "Date")]
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		[Column(TypeName = "Date")]
		public DateTime? LastLoginDate { get; set; }
	}
}
