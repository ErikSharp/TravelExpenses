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

namespace TravelExpenses.Application.Features.Categories
{
    public class CreateCategory
    {
        public class Query : IRequest<CategoryOut[]>
        {
            public Query(Category[] categories)
            {
                Categories = categories;
            }

            public Category[] Categories { get; }
        }

        public class Handler : IRequestHandler<Query, CategoryOut[]>
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

            public async Task<CategoryOut[]> Handle(Query request, CancellationToken cancellationToken)
            {
                context.Categories.AddRange(request.Categories);
                await context.SaveChangesAsync().ConfigureAwait(false);

                var userId = request.Categories.First().UserId;

                var categories = await context.Categories
                    .Where(c => c.UserId == userId)
                    .Include(c => c.User)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return categories.Select(c => mapper.Map<CategoryOut>(c)).ToArray();
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleForEach(c => c.Categories).SetValidator(new CategoryValidator());
            }

            public class CategoryValidator : AbstractValidator<Category>
            {
                public CategoryValidator()
                {
                    RuleFor(c => c.CategoryName).NotEmpty().Length(3, 255);
                    RuleFor(c => c.CategoryName)
                        .Must(name => name.ToUpperInvariant() != "Loss/Gain".ToUpperInvariant())
                        .WithMessage("The category must not be named 'Loss/Gain' in any case");
                }
            }            
        }        
    }
}
