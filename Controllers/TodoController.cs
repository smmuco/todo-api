using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Interface;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<TodoController> _logger;
        
        public TodoController(ITodoRepository todoRepository, ILogger<TodoController> logger)
        {
            _todoRepository = todoRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var todos = await _todoRepository.GetAllAsync();
                return Ok(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching todo items.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var todo = await _todoRepository.GetByIdAsync(id);
                if (todo == null)
                {
                    return NotFound();
                }
                return Ok(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the todo item with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoItem todoItem)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _todoRepository.AddAsync(todoItem);
                    return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, todoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new todo item.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoItem todoItem)
        {
            try
            {
                if (id != todoItem.Id || !ModelState.IsValid)
                {
                    return BadRequest("Invalid todo item.");
                }
                var existingTodo = await _todoRepository.GetByIdAsync(id);
                if (existingTodo == null)
                {
                    return NotFound();
                }
                
                existingTodo.Description = todoItem.Description;
                existingTodo.Title = todoItem.Title;
                existingTodo.IsCompleted = todoItem.IsCompleted;

                await _todoRepository.UpdateAsync(existingTodo);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the todo item with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingTodo = await _todoRepository.GetByIdAsync(id);
                if (existingTodo == null)
                {
                    return NotFound();
                }
                await _todoRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the todo item with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
