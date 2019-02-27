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
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Categories
{
    public class UpdateCategory
    {
        public class Query : IRequest<CategoryOut[]>
        {
            public Query(CategoryUpdateIn category)
            {
                Category = category;
            }

            public CategoryUpdateIn Category { get; }
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
                var category = await context.Categories.Where(k =>
                    k.UserId == request.Category.UserId &&
                    k.Id == request.Category.Id)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (category != null)
                {
                    category.CategoryName = request.Category.CategoryName;
                    category.Icon = request.Category.Icon;
                    category.Color = request.Category.Color;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    var msg = $"Category {request.Category.Id} not found for user {request.Category.UserId}";
                    throw new NotFoundException(msg);
                }

                var categories = await context.Categories
                    .Where(c => c.UserId == request.Category.UserId)
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
                RuleFor(c => c.Category.CategoryName).NotEmpty().Length(3, 255);
            }
        }
    }
}
