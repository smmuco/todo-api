using Microsoft.EntityFrameworkCore;

namespace TaskManager.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }
        public DbSet<Models.TodoItem> TodoItems { get; set; }
        public DbSet<Models.User> Users { get; set; }
    }
}
