using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Interfaces;

namespace ToDoApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoService _todoService;

    public ToDoController(IToDoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoDto>>> GetAll()
    {
        var todos = await _todoService.GetAllTodosAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoDto>> GetById(int id)
    {
        var todo = await _todoService.GetTodoByIdAsync(id);
        return todo != null ? Ok(todo) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<ToDoDto>> Create(ToDoDto todoDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdTodo = await _todoService.AddTodoAsync(todoDto);
        return CreatedAtAction(nameof(GetById), new { id = createdTodo.Id }, createdTodo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ToDoDto todoDto)
    {
        try
        {
            await _todoService.UpdateTodoAsync(id, todoDto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("toggle/{id}")]
    public async Task<IActionResult> ToggleUpdate(int id)
    {
        try
        {
            await _todoService.ToggleStatusTodoAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _todoService.DeleteTodoAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}