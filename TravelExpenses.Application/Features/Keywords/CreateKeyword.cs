using AutoMapper;
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
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Keywords
{
    public class CreateKeyword
    {
        public class Query : IRequest<KeywordOut[]>
        {
            public Query(Keyword[] keywords)
            {
                Keywords = keywords;
            }

            public Keyword[] Keywords { get; }
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

            public async Task<KeywordOut[]> Handle(Query request, CancellationToken cancellationToken)
            {
                context.Keywords.AddRange(request.Keywords);
                await context.SaveChangesAsync().ConfigureAwait(false);

                var userId = request.Keywords.First().UserId;

                var keywords = await context.Keywords
                    .Where(k => k.UserId == userId)
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
                RuleForEach(c => c.Keywords).SetValidator(new KeywordValidator());
            }

            public class KeywordValidator : AbstractValidator<Keyword>
            {
                public KeywordValidator()
                {
                    RuleFor(c => c.KeywordName).NotEmpty().Length(3, 255);
                }
            }
        }
    }
}
