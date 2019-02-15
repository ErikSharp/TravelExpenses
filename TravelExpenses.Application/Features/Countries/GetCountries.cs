﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Countries
{
    public class GetCountries
    {
        public class Query : IRequest<List<CountryOut>>
        {
        }

        public class Handler : IRequestHandler<Query, List<CountryOut>>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;

            public Handler(
                TravelExpensesContext context,
                IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<List<CountryOut>> Handle(Query request, CancellationToken cancellationToken)
            {
                var countries = await context.Countries
                    .ToListAsync()
                    .ConfigureAwait(false);

                var countriesOut = countries.Select(c => mapper.Map<CountryOut>(c)).ToList();
                return countriesOut;
            }
        }
    }
}
