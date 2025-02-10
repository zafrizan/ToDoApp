using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Interfaces;
using ToDoApp.Core.Entities;
using ToDoApp.Core.Interfaces;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
namespace ToDoApp.Application.Services;

public class ToDoService : IToDoService
{
    private readonly IToDoRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<ToDoDto> _validator;

    public ToDoService(IToDoRepository repository, IMapper mapper, IValidator<ToDoDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IEnumerable<ToDoDto>> GetAllTodosAsync()
    {
        var items = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ToDoDto>>(items);
    }

    public async Task<ToDoDto> GetTodoByIdAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return _mapper.Map<ToDoDto>(item);
    }

    public async Task<ToDoDto> AddTodoAsync(ToDoDto todoDto)
    {
        var validationResult = await _validator.ValidateAsync(todoDto);
        if (!validationResult.IsValid)
        {
            // Explicitly use FluentValidation's ValidationException
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var item = _mapper.Map<ToDoItem>(todoDto);
        var addedItem = await _repository.AddAsync(item);
        return _mapper.Map<ToDoDto>(addedItem);
    }

    public async Task UpdateTodoAsync(int id, ToDoDto todoDto)
    {
        if (id != todoDto.Id)
            throw new ArgumentException("ID mismatch between route and request body");

        var existingItem = await _repository.GetByIdAsync(id);
        if (existingItem == null)
            throw new KeyNotFoundException("Todo item not found.");

        _mapper.Map(todoDto, existingItem);
        await _repository.UpdateAsync(existingItem);
    }

    public async Task ToggleStatusTodoAsync(int id)
    {
        var existingItem = await _repository.GetByIdAsync(id);
        if (existingItem == null)
            throw new KeyNotFoundException("Todo item not found.");

        // Toggle the IsCompleted status
        existingItem.IsCompleted = !existingItem.IsCompleted;

        await _repository.UpdateStatusAsync(existingItem);
    }

    public async Task DeleteTodoAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
            throw new KeyNotFoundException("Todo item not found.");

        await _repository.DeleteAsync(item);
    }
}
