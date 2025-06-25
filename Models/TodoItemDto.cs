using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title cannot be empty")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        [MinLength(1,ErrorMessage ="Title cannot less then 1 character")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
