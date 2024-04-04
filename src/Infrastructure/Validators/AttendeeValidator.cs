using Domain.Entities;
using Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Validators;

public class AttendeeValidator : AbstractValidator<Attendee>
{
    private readonly IStringLocalizer<ErrorMessages> _stringLocalizer;
    public AttendeeValidator(IStringLocalizer<ErrorMessages> stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        RuleFor(entity => entity.Name)
            .NotEmpty()
            .WithMessage(_stringLocalizer["NameInvalid"]);

        RuleFor(entity => entity.Email)
            .EmailAddress()
            .WithMessage(_stringLocalizer["EmailInvalid"]);
    }
}