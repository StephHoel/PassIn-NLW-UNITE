using Domain.Entities;
using Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Validators;

public class AttendeeValidator : AbstractValidator<Attendee>
{
    public AttendeeValidator(IStringLocalizer<ErrorMessages> stringLocalizer)
    {
        RuleFor(entity => entity.Name)
            .NotEmpty()
            .WithMessage(stringLocalizer["NameInvalid"]);

        RuleFor(entity => entity.Email)
            .EmailAddress()
            .WithMessage(stringLocalizer["EmailInvalid"]);
    }
}