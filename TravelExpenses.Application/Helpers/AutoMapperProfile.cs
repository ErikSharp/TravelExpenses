using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Application.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserIn, User>();
            CreateMap<User, UserOut>();
        }
    }
}
