using System.ComponentModel.DataAnnotations;

namespace UserService.Dtos
{
    public class UserLoginDto
    {
        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }
    }
}
