using System.ComponentModel.DataAnnotations;

namespace ReviewService.Dtos
{
    public class ReviewUpdateDto
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(2000)]
        public string Comment { get; set; }
    }
}
