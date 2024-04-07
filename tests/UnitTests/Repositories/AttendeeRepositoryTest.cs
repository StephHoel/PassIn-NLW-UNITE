using AutoMapper;
using Communication.Requests;
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

public class AttendeeRepositoryTest
{
    private readonly PassInDbContext _dbContext;
    private readonly LocalizedString _nameInvalid;
    private readonly LocalizedString _emailInvalid;
    private readonly AttendeeRepository _repository;
    private readonly Guid _eventId;
    private readonly Guid _attendeeId;

    private readonly Mock<IMapper> _mapper;

    private readonly Mock<IStringLocalizer<ErrorMessages>> _mockStringLocalizer;

    public AttendeeRepositoryTest()
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
        _mapper.Setup(m => m.Map<Attendee>(It.IsAny<RequestRegisterEventJson>()))
             .Returns((RequestRegisterEventJson source) => mapperConfig.CreateMapper().Map<Attendee>(source));
        _mapper.Setup(m => m.Map<ResponseRegisteredJson>(It.IsAny<Attendee>()))
             .Returns((Attendee source) => mapperConfig.CreateMapper().Map<ResponseRegisteredJson>(source));
        _mapper.Setup(m => m.Map<List<Attendee>, ResponseAllAttendeesJson>(It.IsAny<List<Attendee>>()))
             .Returns((List<Attendee> source) => mapperConfig.CreateMapper().Map<List<Attendee>, ResponseAllAttendeesJson>(source));

        _nameInvalid = new LocalizedString("nameInvalid", "Name is invalid");
        _emailInvalid = new LocalizedString("emailInvalid", "Email is invalid");

        _mockStringLocalizer = new();
        _mockStringLocalizer
            .Setup(sl => sl["NameInvalid"])
            .Returns(_nameInvalid);
        _mockStringLocalizer
            .Setup(sl => sl["EmailInvalid"])
            .Returns(_emailInvalid);

        _repository = new AttendeeRepository(_dbContext, _mapper.Object, _mockStringLocalizer.Object);

        _eventId = Guid.NewGuid();
        _attendeeId = Guid.NewGuid();

        var eventEntity = new Event() { Id = _eventId, Maximum_Attendees = 2 };

        _dbContext.Events.Add(eventEntity);
        _dbContext.SaveChanges();

        var attendeeEntity = new Attendee()
        {
            Id = _attendeeId,
            Name = "Generic Name",
            Email = "generic@email.com",
            Event_Id = _eventId,
            Created_At = DateTime.UtcNow,
            CheckIn = new CheckIn()
            {
                Id = Guid.NewGuid(),
                Attendee_Id = _attendeeId,
                Created_at = DateTime.UtcNow,
            },
        };

        _dbContext.Attendees.Add(attendeeEntity);
        _dbContext.SaveChanges();
    }

    [Fact]
    public void CreateNewAttendee_ValidRequest_ShouldReturnRegisteredAttendee()
    {
        // Arrange
        var request = FakeRequestRegisterEventJson.Generate();

        // Act
        var result = _repository.CreateNewAttendee(_eventId, request);

        // Assert
        Assert.NotNull(result);
        result.Id.Should().NotBeEmpty();
        Assert.IsType<ResponseRegisteredJson>(result);
        Assert.IsType<Guid>(result.Id);
    }

    [Fact]
    public void CreateNewAttendee_ValidRequest_ShouldReturnThrow_NoRoomEvent()
    {
        // Arrange
        var request = FakeRequestRegisterEventJson.Generate();

        var attendeeEntity = FakeAttendee.Generate(_eventId, FakeCheckIn.Generate());

        _dbContext.Attendees.Add(attendeeEntity);
        _dbContext.SaveChanges();

        // Act & Assert
        Assert.Throws<ConflictException>(() => _repository.CreateNewAttendee(_eventId, request));
    }

    [Fact]
    public void CreateNewAttendee_ValidRequest_ShouldReturnThrow_EventNotExist()
    {
        // Arrange
        var request = FakeRequestRegisterEventJson.Generate();

        // Act & Assert
        Assert.Throws<NotFoundException>(() => _repository.CreateNewAttendee(Guid.NewGuid(), request));
    }

    [Fact]
    public void CreateNewAttendee_ValidRequest_ShouldReturnThrow_RegisterTwiceSameEvent()
    {
        // Arrange
        var request = new RequestRegisterEventJson()
        {
            Name = "Generic Name",
            Email = "generic@email.com",
        };

        // Act & Assert
        Assert.Throws<ConflictException>(() => _repository.CreateNewAttendee(_eventId, request));
    }

    [Fact]
    public void CreateNewAttendee_ValidRequest_ShouldReturnThrow_NameInvalid()
    {
        // Arrange
        var request = new RequestRegisterEventJson()
        {
            Name = "",
            Email = "generic@email.com",
        };

        // Act & Assert
        Assert.Throws<ErrorOnValidationException>(() => _repository.CreateNewAttendee(_eventId, request));
    }

    [Fact]
    public void GetAllByEventId_ExistingEvent_ShouldReturnAllAttendees()
    {
        // Arrange

        // Act
        var result = _repository.GetAllByEventId(_eventId);

        // Assert
        Assert.NotNull(result);
        result.Attendees.Should().NotBeEmpty();
        result.Attendees.Should().HaveCount(1);
        Assert.IsType<ResponseAllAttendeesJson>(result);
        Assert.IsType<List<ResponseAttendeeJson>>(result.Attendees);
    }

    [Fact]
    public void GetAllByEventId_NotExistingEvent_ShouldReturnThrow_EventNotExist()
    {
        // Act & Assert
        Assert.Throws<NotFoundException>(() => _repository.GetAllByEventId(Guid.NewGuid()));
    }
}