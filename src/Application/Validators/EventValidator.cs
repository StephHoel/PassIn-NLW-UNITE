using Domain.Entities;
using FluentValidation;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators;

public class EventValidator : AbstractValidator<Event>
{
    private readonly PassInDbContext _dbContext;
    public EventValidator(PassInDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(entity => entity.Maximum_Attendees)
                .GreaterThan(0)
                .WithMessage("The maximum attendees is invalid.");

        RuleFor(entity => entity.Title)
                .NotEmpty()
                .WithMessage("The title is invalid");

        RuleFor(entity => entity.Details)
                .NotEmpty()
                .WithMessage("The details is invalid");

        var response = _dbContext.Events.FirstOrDefault(x => x.Slug == x.Slug);
        RuleFor(_ => response)
                .NotNull()
                .WithMessage("This title is already being used. Try another.");

        RuleFor(request => request)
            .SetValidator(new UniqueSlugValidator(_dbContext));
    }
}