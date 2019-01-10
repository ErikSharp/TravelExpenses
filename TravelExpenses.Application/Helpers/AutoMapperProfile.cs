using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.Transactions;
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

            CreateMap<TransactionIn, Transaction>()
                .ForMember(dest => dest.TransDate,
                    e => e.MapFrom(source =>
                        DateTime.ParseExact(source.TransDate, CreateTransaction.DateStringFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.TransactionKeywords,
                    e => e.MapFrom(source => source.KeywordIds.Select(id => new TransactionKeyword { KeywordId = id })));

            CreateMap<Country, CountryOut>();
            CreateMap<Keyword, KeywordOut>();

            CreateMap<Location, LocationOut>()
                .ForMember(dest => dest.CountryName,
                    e => e.MapFrom(source => source.Country.CountryName));
        }
    }
}
