using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash{ get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } // e.g., Admin, User, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
