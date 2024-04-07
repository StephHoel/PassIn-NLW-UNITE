using Domain.Entities;
using Domain.Shared;
using FluentValidation.TestHelper;
using Infrastructure.Validators;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace UnitTests.Validators;

public class AttendeeValidatorTests
{
    private readonly LocalizedString _nameInvalid;
    private readonly LocalizedString _emailInvalid;

    private readonly AttendeeValidator _validator;

    private readonly Mock<IStringLocalizer<ErrorMessages>> _mockStringLocalizer;

    public AttendeeValidatorTests()
    {
        _nameInvalid = new LocalizedString("nameInvalid", "Name is invalid");
        _emailInvalid = new LocalizedString("emailInvalid", "Email is invalid");

        _mockStringLocalizer = new();
        _mockStringLocalizer
            .Setup(sl => sl["NameInvalid"])
            .Returns(_nameInvalid);
        _mockStringLocalizer
            .Setup(sl => sl["EmailInvalid"])
            .Returns(_emailInvalid);

        _validator = new AttendeeValidator(_mockStringLocalizer.Object);
    }

    [Fact]
    public void Name_Validation_Should_Fail_When_Name_Is_Empty()
    {
        // Arrange
        var attendee = new Attendee { Name = string.Empty, };

        // Act
        var result = _validator.TestValidate(attendee);

        // Assert
        Assert.False(result.IsValid);

        result.ShouldHaveValidationErrorFor(e => e.Name)
              .WithErrorMessage(_mockStringLocalizer.Object["NameInvalid"]);
    }

    [Fact]
    public void Email_Validation_Should_Fail_When_Email_Is_Invalid()
    {
        // Arrange
        var attendee = new Attendee { Email = "invalid_email" };

        // Act
        var result = _validator.TestValidate(attendee);

        // Assert
        Assert.False(result.IsValid);

        result.ShouldHaveValidationErrorFor(e => e.Email)
              .WithErrorMessage(_mockStringLocalizer.Object["EmailInvalid"]);
    }

    [Fact]
    public void Validation_Should_Pass_When_Name_And_Email_Are_Valid()
    {
        // Arrange
        var attendee = new Attendee
        {
            Name = "John",
            Email = "john@example.com"
        };

        // Act
        var result = _validator.TestValidate(attendee);

        // Assert
        Assert.True(result.IsValid);

        result.ShouldNotHaveAnyValidationErrors();
    }
}