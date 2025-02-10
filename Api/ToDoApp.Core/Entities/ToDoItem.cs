using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Core.Entities;

public class ToDoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty; // Initialize with empty string
    public string Description { get; set; } = string.Empty; // Initialize with empty string
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
