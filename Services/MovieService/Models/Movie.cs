using MovieService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieService.Models
{
    public class Movie : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(32)]
        public string Title { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        [Required]
        public IEnumerable<Category> Categories { get; set; }

        [Required]
        public ContentType ContentType { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? ReleaseDate { get; set; }

    }
}
