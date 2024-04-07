using Domain.Entities;
using Domain.Shared;
using FluentValidation.TestHelper;
using Infrastructure.Context;
using Infrastructure.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace UnitTests.Validators;

public class EventValidatorTest
{
    private readonly LocalizedString _MaximumAttendeesInvalid;
    private readonly LocalizedString _TitleInvalid;
    private readonly LocalizedString _DetailsInvalid;
    private readonly LocalizedString _TitleUsed;

    private readonly EventValidator _validator;

    private readonly Mock<IStringLocalizer<ErrorMessages>> _mockStringLocalizer;
    private readonly PassInDbContext _dbContext;

    public EventValidatorTest()
    {
        _MaximumAttendeesInvalid = new LocalizedString("MaximumAttendeesInvalid", "Maximum Attendees is Invalid");
        _TitleInvalid = new LocalizedString("TitleInvalid", "Title is invalid");
        _DetailsInvalid = new LocalizedString("DetailsInvalid", "Details is invalid");
        _TitleUsed = new LocalizedString("TitleUsed", "Title was used");

        _mockStringLocalizer = new();
        _mockStringLocalizer
            .Setup(sl => sl["MaximumAttendeesInvalid"])
            .Returns(_MaximumAttendeesInvalid);
        _mockStringLocalizer
            .Setup(sl => sl["TitleInvalid"])
            .Returns(_TitleInvalid);
        _mockStringLocalizer
            .Setup(sl => sl["DetailsInvalid"])
            .Returns(_DetailsInvalid);
        _mockStringLocalizer
            .Setup(sl => sl["TitleUsed"])
            .Returns(_TitleUsed);

        var options = new DbContextOptionsBuilder<PassInDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _dbContext = new PassInDbContext(options);

        _validator = new EventValidator(_dbContext, _mockStringLocalizer.Object);
    }

    [Fact]
    public void Maximum_Attendees_Should_Be_Greater_Than_Zero()
    {
        // Arrange
        var entity = new Event { Maximum_Attendees = 0 };

        // Act
        var result = _validator.TestValidate(entity);

        // Assert
        Assert.False(result.IsValid);

        result.ShouldHaveValidationErrorFor(e => e.Maximum_Attendees)
              .WithErrorMessage(_mockStringLocalizer.Object["MaximumAttendeesInvalid"]);
    }

    [Fact]
    public void Title_Should_Not_Be_Empty()
    {
        // Arrange
        var entity = new Event { Title = "" };

        // Act
        var result = _validator.TestValidate(entity);

        // Assert
        Assert.False(result.IsValid);

        result.ShouldHaveValidationErrorFor(e => e.Title)
              .WithErrorMessage(_mockStringLocalizer.Object["TitleInvalid"]);
    }

    [Fact]
    public void Details_Should_Not_Be_Empty()
    {
        // Arrange
        var entity = new Event { Details = "" };

        // Act
        var result = _validator.TestValidate(entity);

        // Assert
        Assert.False(result.IsValid);

        result.ShouldHaveValidationErrorFor(e => e.Details)
              .WithErrorMessage(_mockStringLocalizer.Object["DetailsInvalid"]);
    }

    [Fact]
    public void Slug_Should_Be_Unique()
    {
        // Arrange
        var existingSlug = "existing-slug";
        _dbContext.Events.Add(new Event { Slug = existingSlug });
        _dbContext.SaveChanges();

        var entity = new Event { Slug = existingSlug };

        // Act
        var result = _validator.TestValidate(entity);

        // Assert
        Assert.False(result.IsValid);

        result.ShouldHaveValidationErrorFor(e => e.Slug)
              .WithErrorMessage(_mockStringLocalizer.Object["TitleUsed"]);
    }
}