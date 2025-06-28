using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
