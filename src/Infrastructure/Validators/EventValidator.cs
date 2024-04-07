using Domain.Entities;
using Domain.Shared;
using FluentValidation;
using Infrastructure.Context;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Validators;

public class EventValidator : AbstractValidator<Event>
{
    private readonly PassInDbContext _dbContext;

    public EventValidator(PassInDbContext dbContext, IStringLocalizer<ErrorMessages> stringLocalizer)
    {
        _dbContext = dbContext;

        RuleFor(entity => entity.Maximum_Attendees)
            .GreaterThan(0)
            .WithMessage(stringLocalizer["MaximumAttendeesInvalid"]);

        RuleFor(entity => entity.Title)
            .NotEmpty()
            .WithMessage(stringLocalizer["TitleInvalid"]);

        RuleFor(entity => entity.Details)
            .NotEmpty()
            .WithMessage(stringLocalizer["DetailsInvalid"]);

        RuleFor(entity => entity.Slug)
            .Must(BeUniqueSlug)
            .WithMessage(stringLocalizer["TitleUsed"]);
    }

    private bool BeUniqueSlug(string slug)
    {
        return !_dbContext.Events.Any(e => e.Slug == slug);
    }
}