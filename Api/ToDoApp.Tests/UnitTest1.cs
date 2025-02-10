using AutoMapper;
using FluentAssertions;
using FluentValidation; // Added
using Moq;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Services;
using ToDoApp.Application.Validators; // Validator namespace
using ToDoApp.Core.Entities;
using ToDoApp.Core.Interfaces;
using Xunit;

namespace ToDoApp.Tests;

public class ToDoServiceTests
{
    private readonly Mock<IToDoRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly IValidator<ToDoDto> _validator;
    private readonly IToDoService _service;

    public ToDoServiceTests()
    {
        _mockRepo = new Mock<IToDoRepository>();

        // Configure AutoMapper
        var config = new MapperConfiguration(cfg =>
            cfg.AddProfile<ToDoApp.Application.Mappings.MappingProfile>());
        _mapper = config.CreateMapper();

        // Add validator
        _validator = new ToDoDtoValidator();

        // Initialize service with all dependencies
        _service = new ToDoService(_mockRepo.Object, _mapper, _validator);
    }

    [Fact]
    public async Task AddTodoAsync_ValidTodo_ReturnsTodoDto()
    {
        // Arrange
        var todoDto = new ToDoDto { Title = "Test Todo", Description = "Test Description" };
        var todoItem = _mapper.Map<ToDoItem>(todoDto);

        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<ToDoItem>()))
                 .ReturnsAsync(todoItem);

        // Act
        var result = await _service.AddTodoAsync(todoDto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(todoDto.Title);
        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<ToDoItem>()), Times.Once);
    }

    [Fact]
    public async Task AddTodoAsync_EmptyTitle_ThrowsValidationException()
    {
        // Arrange
        var invalidTodo = new ToDoDto { Title = "", Description = "Invalid" };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
            _service.AddTodoAsync(invalidTodo));
        ex.Errors.Should().Contain(e => e.ErrorMessage == "Title is required");
    }

    [Fact]
    public async Task UpdateTodoAsync_InvalidId_ThrowsArgumentException()
    {
        // Arrange
        var invalidTodo = new ToDoDto { Id = 1, Title = "Test" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UpdateTodoAsync(2, invalidTodo)); // ID mismatch
    }

    [Fact]
    public async Task UpdateTodoAsync_NonExistentTodo_ThrowsKeyNotFoundException()
    {
        // Arrange
        var nonExistentTodo = new ToDoDto { Id = 999, Title = "Ghost Todo" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(999))
                 .ReturnsAsync((ToDoItem)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateTodoAsync(999, nonExistentTodo));
    }

    [Fact]
    public async Task DeleteTodoAsync_ValidId_DeletesSuccessfully()
    {
        // Arrange
        var existingItem = new ToDoItem { Id = 1, Title = "Existing Todo" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(1))
                 .ReturnsAsync(existingItem);

        // Act
        await _service.DeleteTodoAsync(1);

        // Assert
        _mockRepo.Verify(repo => repo.DeleteAsync(existingItem), Times.Once);
    }
}