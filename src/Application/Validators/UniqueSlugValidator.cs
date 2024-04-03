using Domain.Entities;
using FluentValidation;
using FluentValidation.Validators;
using Infrastructure.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;

namespace Application.Validators; 

public class UniqueSlugValidator : AbstractValidator<Event>
{
    private readonly PassInDbContext _dbContext;

    public UniqueSlugValidator(PassInDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(entity => entity.Slug)
            .Must(BeUniqueSlug)
            .WithMessage("This title is already being used. Try another.");
    }

    private bool BeUniqueSlug(string slug)
    {
        return !_dbContext.Events.Any(e => e.Slug == slug);
    }
}

