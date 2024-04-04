using Domain.Entities;
using Domain.Shared;
using FluentValidation;
using Infrastructure.Context;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Validators;

public class EventValidator : AbstractValidator<Event>
{
    private readonly PassInDbContext _dbContext;
    private readonly IStringLocalizer<ErrorMessages> _stringLocalizer;

    public EventValidator(PassInDbContext dbContext, IStringLocalizer<ErrorMessages> stringLocalizer)
    {
        _dbContext = dbContext;
        _stringLocalizer = stringLocalizer;

        RuleFor(entity => entity.Maximum_Attendees)
            .GreaterThan(0)
            .WithMessage(_stringLocalizer["MaximumAttendeesInvalid"]);

        RuleFor(entity => entity.Title)
            .NotEmpty()
            .WithMessage(_stringLocalizer["TitleInvalid"]);

        RuleFor(entity => entity.Details)
            .NotEmpty()
            .WithMessage(_stringLocalizer["DetailsInvalid"]);

        RuleFor(entity => entity.Slug)
            .Must(BeUniqueSlug)
            .WithMessage(_stringLocalizer["TitleUsed"]);
    }

    private bool BeUniqueSlug(string slug)
    {
        return !_dbContext.Events.Any(e => e.Slug == slug);
    }
}