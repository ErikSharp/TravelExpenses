using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            CreateMap<UserRegistration, User>();
            CreateMap<User, UserOut>();
            CreateMap<TransactionIn, Transaction>().ForMember(ti => 
                ti.TransDate, 
                e => e.MapFrom(o => 
                    DateTime.ParseExact(o.TransDate, "yyyyMMdd", CultureInfo.InvariantCulture)));
        }
    }
}
