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
            public Query(string[] keywords, int userId)
            {
                Keywords = keywords;
                UserId = userId;
            }

            public string[] Keywords { get; }
            public int UserId { get; }
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
                var keywordsIn = request.Keywords
                    .Select(k => new Keyword() { KeywordName = k, UserId = request.UserId })
                    .ToArray();

                context.Keywords.AddRange(keywordsIn);
                await context.SaveChangesAsync().ConfigureAwait(false);
                
                var keywordsOut = await context.Keywords
                    .Where(k => k.UserId == request.UserId)
                    .Include(k => k.User)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return keywordsOut.Select(k => mapper.Map<KeywordOut>(k)).ToArray();
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleForEach(c => c.Keywords).SetValidator(new KeywordValidator());
            }

            public class KeywordValidator : AbstractValidator<string>
            {
                public KeywordValidator()
                {
                    RuleFor(c => c).NotEmpty().Length(3, 255);
                }
            }
        }
    }
}
