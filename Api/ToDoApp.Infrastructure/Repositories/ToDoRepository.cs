using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToDoApp.Core.Entities;
using ToDoApp.Core.Interfaces;
using ToDoApp.Infrastructure.Data;

namespace ToDoApp.Infrastructure.Repositories;

public class ToDoRepository : IToDoRepository
{
    private readonly ToDoDbContext _context;

    public ToDoRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<ToDoItem> GetByIdAsync(int id) => await _context.ToDoItems.FindAsync(id);

    public async Task<IEnumerable<ToDoItem>> GetAllAsync() => await _context.ToDoItems.ToListAsync();

    public async Task<ToDoItem> AddAsync(ToDoItem item)
    {
        await _context.ToDoItems.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task UpdateAsync(ToDoItem item)
    {
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(ToDoItem item)
    {
        _context.Attach(item);
        _context.Entry(item).Property(x => x.IsCompleted).IsModified = true;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ToDoItem item)
    {
        _context.ToDoItems.Remove(item);
        await _context.SaveChangesAsync();
    }
}
