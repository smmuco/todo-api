using Moq;
using TaskManager.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.API.Interface;
using TaskManager.API.Models;



namespace TaskManager.Tests
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoRepository> _mockRepo;
        private readonly Mock<ILogger<TodoController>> _mockLogger;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockRepo = new Mock<ITodoRepository>();
            _mockLogger = new Mock<ILogger<TodoController>>();
            _controller = new TodoController(_mockRepo.Object, _mockLogger.Object);
        }

        // Should return 200 OK when a valid ID is provided
        [Fact]
        public async Task GetById_ReturnsOkResult_WhenTodoExists()
        {
            // Arrange
            var todoId = 1;
            var todoItem = new TodoItem { Id = todoId, Title = "Test Todo", Description = "Test Description"};

            _mockRepo.Setup(repo => repo.GetByIdAsync(todoId))
                .ReturnsAsync(todoItem);
            // Act
            var result = await _controller.GetById(todoId);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTodo = Assert.IsType<TodoItemDto>(okResult.Value);
            Assert.Equal(todoId, returnedTodo.Id);
        }

        // Should return 404 NotFound when the ID does not exist
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            // Arrange
            var todoId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(todoId))
                .ReturnsAsync((TodoItem)null);
            // Act
            var result = await _controller.GetById(todoId);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Should return 200 OK with a list of TodoItems when GetAll is called
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfTodos()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Test Todo 1", Description = "Test Description 1" },
                new TodoItem { Id = 2, Title = "Test Todo 2", Description = "Test Description 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(todoItems);
            // Act
            var result = await _controller.GetAll();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTodos = Assert.IsType<List<TodoItemDto>>(okResult.Value);
            Assert.Equal(2, returnedTodos.Count);
        }

        // Should return 200 OK with an empty list when there are no TodoItems
        [Fact]
        public async Task GetAll_ReturnOkResult_WithoutListOfTodos()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<TodoItem>());
            // Act
            var result = await _controller.GetAll();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTodos = Assert.IsType<List<TodoItemDto>>(okResult.Value);
            Assert.Empty(returnedTodos);
        }

        // Should return 201 CreatedAtAction when a new TodoItem is created
        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenTodoIsCreated()
        {
            // Arrange
            var newTodo = new TodoItemDto { Id=1,Title = "New Todo", Description = "New Description" };
            var createdTodo = new TodoItem { Id = 1, Title = newTodo.Title, Description = newTodo.Description };

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>()))
         .Returns(Task.CompletedTask);
            // Act
            var result = await _controller.Create(newTodo);
            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedTodo = Assert.IsType<TodoItemDto>(createdResult.Value);
            Assert.Equal(createdTodo.Id, returnedTodo.Id);
        }

        // Should return 400 BadRequest when the TodoItem is null
        [Fact]
        public async Task Create_ReturnsNotCreated_WhenTodoIsNull()
        {
            // Arrange
            TodoItemDto? newTodo = null;
            // Act
            var result = await _controller.Create(newTodo);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // Should return 204 NoContent when a TodoItem is successfully updated
        [Fact]
        public async Task Update_ReturnsOk_WhenTodoIsUpdated()
        {
            // Arrange
            var updatedTodo = new TodoItemDto { Id = 1, Title = "Updated Todo", Description = "Updated Description" };
            var existingTodo = new TodoItem { Id = updatedTodo.Id, Title = "Old Todo", Description = "Old Description" };
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<TodoItem>()))
                .Returns(Task.CompletedTask);
            _mockRepo.Setup(repo => repo.GetByIdAsync(existingTodo.Id)).ReturnsAsync(existingTodo);
            // Act
            var result = await _controller.Update(updatedTodo.Id,updatedTodo);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Should return 404 NotFound when trying to update a TodoItem that does not exist
        [Fact]
        public async Task Update_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            // Arrange
            var updatedTodo = new TodoItemDto { Id = 1, Title = "Updated Todo", Description = "Updated Description" };
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<TodoItem>()))
                .Throws(new KeyNotFoundException("Todo not found"));
            // Act
            var result = await _controller.Update(updatedTodo.Id,updatedTodo);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Should return 204 NoContent when a TodoItem is successfully deleted
        [Fact]
        public async Task Delete_ReturnsNoContent_WhenTodoIsDeleted()
        {
            // Arrange
            var todoId = 1;
            _mockRepo.Setup(repo => repo.DeleteAsync(todoId))
                .Returns(Task.CompletedTask);
            _mockRepo.Setup(repo => repo.GetByIdAsync(todoId)).ReturnsAsync(new TodoItem { Id = todoId, Title = "Test Todo", Description = "Test Description" });
            // Act
            var result = await _controller.Delete(todoId);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Should return 404 NotFound when trying to delete a TodoItem that does not exist
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            // Arrange
            var todoId = 1;
            _mockRepo.Setup(repo => repo.DeleteAsync(todoId))
                .Throws(new KeyNotFoundException("Todo not found"));
            // Act
            var result = await _controller.Delete(todoId);
            // Assert
            Assert.IsType<NotFoundResult>(result);

            //lkjklhghklgh
        }
    }
}