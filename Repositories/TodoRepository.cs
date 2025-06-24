using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.Interface;
using TaskManager.API.Models;

namespace TaskManager.API.Repositories
{
    public class TodoRepository : ITodoRepository
    {
            private readonly ApplicationDbContext _context;
            public TodoRepository(ApplicationDbContext context)
            {
                _context = context;
            }
        
        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            try
            {
                return await _context.TodoItems.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanı hatası: {ex.Message}");
                throw;
            }
            //return await _context.TodoItems.ToListAsync();
        }
        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }
        public async Task AddAsync(TodoItem item)
        {
            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TodoItem item)
        {
            _context.TodoItems.Update(item);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item != null)
            {
                _context.TodoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
