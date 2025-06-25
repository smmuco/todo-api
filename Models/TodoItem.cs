using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Title cannot be empty")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
