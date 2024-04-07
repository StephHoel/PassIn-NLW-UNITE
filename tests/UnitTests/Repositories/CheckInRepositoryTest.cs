using AutoMapper;
using Communication.Responses;
using Domain.Entities;
using Domain.Shared;
using Exceptions;
using FluentAssertions;
using Infrastructure.AutoMapper;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using UnitTests.FakeObjects;
using Xunit;

namespace UnitTests.Repositories;

public class CheckInRepositoryTest
{
    private readonly PassInDbContext _dbContext;
    private readonly LocalizedString _AttendeeNotFound;
    private readonly LocalizedString _AttendeeTwiceChecking;
    private readonly CheckInRepository _repository;
    private readonly Guid _eventId;
    private readonly Guid _attendeeId;

    private readonly Mock<IMapper> _mapper;

    private readonly Mock<IStringLocalizer<ErrorMessages>> _mockStringLocalizer;

    public CheckInRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<PassInDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _dbContext = new PassInDbContext(options);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
            cfg.AddProfile<CheckInMapper>();
        });
        _mapper = new();
        _mapper.Setup(m => m.Map<CheckIn>(It.IsAny<Guid>()))
             .Returns((Guid source) => mapperConfig.CreateMapper().Map<CheckIn>(source));
        _mapper.Setup(m => m.Map<ResponseRegisteredJson>(It.IsAny<CheckIn>()))
             .Returns((CheckIn source) => mapperConfig.CreateMapper().Map<ResponseRegisteredJson>(source));

        _AttendeeNotFound = new LocalizedString("AttendeeNotFound", "Attendee not found");
        _AttendeeTwiceChecking = new LocalizedString("AttendeeTwiceChecking", "Attendee cannot check twice in same event");

        _mockStringLocalizer = new();
        _mockStringLocalizer
            .Setup(sl => sl["AttendeeNotFound"])
            .Returns(_AttendeeNotFound);
        _mockStringLocalizer
            .Setup(sl => sl["AttendeeTwiceChecking"])
            .Returns(_AttendeeTwiceChecking);

        _repository = new CheckInRepository(_dbContext, _mapper.Object, _mockStringLocalizer.Object);

        _eventId = Guid.NewGuid();
        _attendeeId = Guid.NewGuid();

        var eventEntity = new Event() { Id = _eventId, Maximum_Attendees = 2 };

        _dbContext.Events.Add(eventEntity);
        _dbContext.SaveChanges();
    }

    [Fact]
    public void DoCheckIn_ValidId_ShouldReturn_ResponseRegisteredJson()
    {
        // Arrange
        var attendeeEntity = FakeAttendee.Generate(_eventId, _attendeeId, null);

        _dbContext.Attendees.Add(attendeeEntity);
        _dbContext.SaveChanges();

        // Act
        var result = _repository.DoCheckIn(_attendeeId);

        // Assert
        Assert.NotNull(result);
        result.Id.Should().NotBeEmpty();
        Assert.IsType<ResponseRegisteredJson>(result);
        Assert.IsType<Guid>(result.Id);
    }

    [Fact]
    public void DoCheckIn_ValidId_ShouldReturnThrow_AttendeeTwiceChecking()
    {
        // Arrange
        var attendeeEntity = FakeAttendee.Generate(_eventId, _attendeeId, FakeCheckIn.Generate(_attendeeId));

        _dbContext.Attendees.Add(attendeeEntity);
        _dbContext.SaveChanges();

        // Act & Assert
        Assert.Throws<ConflictException>(() => _repository.DoCheckIn(_attendeeId));
    }

    [Fact]
    public void DoCheckIn_ValidId_ShouldReturnThrow_AttendeeNotFound()
    {
        // Act & Assert
        Assert.Throws<NotFoundException>(() => _repository.DoCheckIn(Guid.NewGuid()));
    }
}