using System.ComponentModel.DataAnnotations;

namespace MovieService.Dtos.CategoryDto
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
    }
}
