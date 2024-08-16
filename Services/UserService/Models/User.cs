using Microsoft.AspNetCore.Identity;
using MovieReviewApp.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
	public class User : IdentityUser<Guid>, IEntity
	{
		[Column(TypeName = "Date")]
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	}
}
