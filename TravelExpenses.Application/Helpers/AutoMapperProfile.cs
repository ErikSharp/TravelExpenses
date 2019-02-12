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

            CreateMap<TransactionCreateIn, Transaction>()
                .ForMember(dest => dest.TransDate,
                    e => e.MapFrom(source =>
                        DateTime.ParseExact(source.TransDate, CreateTransaction.DateStringFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.TransactionKeywords,
                    e => e.MapFrom(source => source.KeywordIds.Select(id => new TransactionKeyword { KeywordId = id })));

            CreateMap<TransactionEditIn, Transaction>()
                .ForMember(dest => dest.TransDate,
                    e => e.MapFrom(source =>
                        DateTime.ParseExact(source.TransDate, CreateTransaction.DateStringFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.TransactionKeywords,
                    e => e.MapFrom(source => source.KeywordIds.Select(id => new TransactionKeyword { KeywordId = id })));

            CreateMap<Transaction, TransactionOut>()
                .ForMember(dest => dest.TransDate,
                    e => e.MapFrom(source =>
                        source.TransDate.ToString(CreateTransaction.DateStringFormat)))
                .ForMember(dest => dest.KeywordIds,
                    e => e.MapFrom(source =>
                        source.TransactionKeywords.Select(tk => tk.KeywordId)));

            CreateMap<CashWithdrawalCreateIn, CashWithdrawal>()
                .ForMember(dest => dest.TransDate,
                    e => e.MapFrom(source =>
                        DateTime.ParseExact(source.TransDate, CreateTransaction.DateStringFormat, CultureInfo.InvariantCulture)));

            CreateMap<CashWithdrawal, CashWithdrawalDto>();
            CreateMap<CashWithdrawalDto, CashWithdrawal>();

            CreateMap<Country, CountryOut>();
            CreateMap<Currency, CurrencyOut>();
            CreateMap<Keyword, KeywordOut>();

            CreateMap<Location, LocationOut>()
                .ForMember(dest => dest.CountryName,
                    e => e.MapFrom(source => source.Country.CountryName));
        }
    }
}
