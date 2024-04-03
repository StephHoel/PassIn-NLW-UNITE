using Domain.Entities;
using FluentValidation;
using Infrastructure.Context;

namespace Application.Validators;

public class AttendeeValidator : AbstractValidator<Attendee>
{
    private readonly PassInDbContext _dbContext;

    public AttendeeValidator(PassInDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(entity => entity.Name)
                .NotEmpty()
                .WithMessage("The name is invalid");

        RuleFor(entity => entity.Email)
                .EmailAddress()
                .WithMessage("The email is invalid");
    }
}