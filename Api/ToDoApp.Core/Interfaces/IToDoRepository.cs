using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Core.Entities;

namespace ToDoApp.Core.Interfaces;

public interface IToDoRepository
{
    Task<ToDoItem?> GetByIdAsync(int id);
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem> AddAsync(ToDoItem item);
    Task UpdateAsync(ToDoItem item);
    Task UpdateStatusAsync(ToDoItem item);
    Task DeleteAsync(ToDoItem item);
}
