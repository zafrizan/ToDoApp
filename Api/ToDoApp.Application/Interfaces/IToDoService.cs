using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToDoApp.Application.Dtos;

namespace ToDoApp.Application.Interfaces;

public interface IToDoService
{
    Task<IEnumerable<ToDoDto>> GetAllTodosAsync();
    Task<ToDoDto> GetTodoByIdAsync(int id);
    Task<ToDoDto> AddTodoAsync(ToDoDto todoDto);
    Task UpdateTodoAsync(int id, ToDoDto todoDto);
    Task ToggleStatusTodoAsync(int id);

    Task DeleteTodoAsync(int id);
}
