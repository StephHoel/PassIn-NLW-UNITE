using Domain.Entities;
using FluentValidation;

namespace Infrastructure.Validators;

public class AttendeeValidator : AbstractValidator<Attendee>
{
    public AttendeeValidator()
    {
        RuleFor(entity => entity.Name)
                .NotEmpty()
                .WithMessage("The name is invalid");

        RuleFor(entity => entity.Email)
                .EmailAddress()
                .WithMessage("The email is invalid");
    }
}