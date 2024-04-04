using Domain.Entities;
using FluentValidation;
using Infrastructure.Context;

namespace Infrastructure.Validators;

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

        RuleFor(entity => entity.Slug)
            .Must(BeUniqueSlug)
            .WithMessage("This title is already being used. Try another.");
    }

    private bool BeUniqueSlug(string slug)
    {
        return !_dbContext.Events.Any(e => e.Slug == slug);
    }
}