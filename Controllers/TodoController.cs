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
        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _todoRepository.GetAllAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid todo item.");
            }
            await _todoRepository.AddAsync(todoItem);
            return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoItem todoItem)
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
            await _todoRepository.UpdateAsync(todoItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingTodo = await _todoRepository.GetByIdAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }
            await _todoRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
