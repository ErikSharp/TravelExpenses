using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Categories
{
    public class CreateCategory
    {
        public class Command : IRequest
        {
            public Command(Category[] categories)
            {
                Categories = categories;
            }

            public Category[] Categories { get; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TravelExpensesContext context;

            public Handler(
                TravelExpensesContext context)
            {
                this.context = context;
            }

            protected override Task Handle(Command request, CancellationToken response)
            {
                context.Categories.AddRange(request.Categories);
                return context.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
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
