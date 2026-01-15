using System.ComponentModel.DataAnnotations;

namespace server.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]

        public string Password { get; set; }
        [Required]

        public string Role { get; set; }

        public string Email { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; } 


    }
}
