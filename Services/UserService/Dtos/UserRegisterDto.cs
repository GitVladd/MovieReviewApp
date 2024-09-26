using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(64)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(64)]
        [MinLength(6)]
        public string Password { get; set; }
    }
}