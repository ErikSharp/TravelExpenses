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
            public Query(CategoryIn[] categories)
            {
                Categories = categories;
            }

            public CategoryIn[] Categories { get; }
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
                var categories = request.Categories.Select(c => mapper.Map<Category>(c));
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync().ConfigureAwait(false);

                var userId = request.Categories.First().UserId;

                var allCategories = await context.Categories
                    .Where(c => c.UserId == userId)
                    .Include(c => c.User)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return allCategories.Select(c => mapper.Map<CategoryOut>(c)).ToArray();
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleForEach(c => c.Categories).SetValidator(new CategoryValidator());
            }

            public class CategoryValidator : AbstractValidator<CategoryIn>
            {
                public CategoryValidator()
                {
                    RuleFor(c => c.CategoryName).NotEmpty().Length(3, 255);
                    RuleFor(c => c.Icon).MaximumLength(40);
                }
            }            
        }        
    }
}
