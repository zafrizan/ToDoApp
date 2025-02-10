using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using ToDoApp.Application.Dtos;
using ToDoApp.Core.Entities;

namespace ToDoApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ToDoItem, ToDoDto>();
        CreateMap<ToDoDto, ToDoItem>();
    }
}
