﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Categories
{
    public class UpdateCategory
    {
        public class Command : IRequest
        {
            public Command(Category category)
            {
                Category = category;
            }

            public Category Category { get; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TravelExpensesContext context;

            public Handler(
                TravelExpensesContext context)
            {
                this.context = context;
            }

            protected override async Task Handle(Command request, CancellationToken response)
            {
                var category = await context.Categories.Where(k =>
                    k.UserId == request.Category.UserId &&
                    k.Id == request.Category.Id)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (category != null)
                {
                    category.CategoryName = request.Category.CategoryName;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    var msg = $"Category {request.Category.Id} not found for user {request.Category.UserId}";
                    throw new NotFoundException(msg);
                }
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Category.CategoryName).NotEmpty().Length(3, 255);
            }
        }
    }
}
