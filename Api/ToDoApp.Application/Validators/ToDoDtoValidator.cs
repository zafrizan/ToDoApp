using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ToDoApp.Application.Dtos;

namespace ToDoApp.Application.Validators;

public class ToDoDtoValidator : AbstractValidator<ToDoDto>
{
    public ToDoDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title must be ≤ 100 characters");
    }
}
