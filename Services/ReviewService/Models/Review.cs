using MovieReviewApp.Common.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewService.Models
{
	public class Review : IEntity
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public Guid MovieId { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[Required]
		[Range(1, 5)]
		public int Rating { get; set; }

        [StringLength(2000)]
        public string Comment { get; set; }

		[Column(TypeName = "Date")]
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	}
}
