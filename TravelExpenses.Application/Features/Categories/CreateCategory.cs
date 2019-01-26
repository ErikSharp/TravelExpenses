using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Categories
{
    public class CreateCategory
    {
        public class Query : IRequest<string[]>
        {
            public Query(Category[] categories)
            {
                Categories = categories;
            }

            public Category[] Categories { get; }
        }

        public class Handler : IRequestHandler<Query, string[]>
        {
            private readonly TravelExpensesContext context;

            public Handler(
                TravelExpensesContext context)
            {
                this.context = context;
            }

            public async Task<string[]> Handle(Query request, CancellationToken cancellationToken)
            {
                context.Categories.AddRange(request.Categories);
                await context.SaveChangesAsync().ConfigureAwait(false);

                var userId = request.Categories.First().UserId;

                var categories = await context.Categories
                    .Where(c => c.UserId == userId)
                    .Include(c => c.User)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return categories.Select(c => c.CategoryName).ToArray();
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
                }
            }
        }        
    }
}
