﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Keywords
{
    public class UpdateKeyword
    {
        public class Query : IRequest<KeywordOut[]>
        {
            public Query(Keyword keyword)
            {
                Keyword = keyword;
            }

            public Keyword Keyword { get; }
        }

        public class Handler : IRequestHandler<Query, KeywordOut[]>
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

            public async Task<KeywordOut[]> Handle(Query request, CancellationToken response)
            {
                var keyword = await context.Keywords.Where(k =>
                    k.UserId == request.Keyword.UserId &&
                    k.Id == request.Keyword.Id)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (keyword != null)
                {
                    keyword.KeywordName = request.Keyword.KeywordName;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    var msg = $"Keyword {request.Keyword.Id} not found for user {request.Keyword.UserId}";
                    throw new NotFoundException(msg);
                }

                var keywords = await context.Keywords
                    .Where(k => k.UserId == request.Keyword.UserId)
                    .Include(k => k.User)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return keywords.Select(k => mapper.Map<KeywordOut>(k)).ToArray();
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(c => c.Keyword.KeywordName).NotEmpty().Length(3, 255);
            }
        }
    }
}
