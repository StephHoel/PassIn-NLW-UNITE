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
using Xunit;

namespace UnitTests.Repositories;

public class EventRepositoryTest
{
    private readonly PassInDbContext _dbContext;
    private readonly LocalizedString _MaximumAttendeesInvalid;
    private readonly LocalizedString _TitleInvalid;
    private readonly LocalizedString _DetailsInvalid;
    private readonly LocalizedString _TitleUsed;
    private readonly EventRepository _repository;
    private readonly Guid _eventId;
    private readonly Guid _attendeeId;

    private readonly Mock<IMapper> _mapper;

    private readonly Mock<IStringLocalizer<ErrorMessages>> _mockStringLocalizer;

    public EventRepositoryTest()
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
        _mapper.Setup(m => m.Map<Event>(It.IsAny<RequestEventJson>()))
             .Returns((RequestEventJson source) => mapperConfig.CreateMapper().Map<Event>(source));
        _mapper.Setup(m => m.Map<ResponseEventJson>(It.IsAny<Event>()))
             .Returns((Event source) => mapperConfig.CreateMapper().Map<ResponseEventJson>(source));
        _mapper.Setup(m => m.Map<ResponseRegisteredJson>(It.IsAny<Event>()))
             .Returns((Event source) => mapperConfig.CreateMapper().Map<ResponseRegisteredJson>(source));

        _MaximumAttendeesInvalid = new LocalizedString("MaximumAttendeesInvalid", "MaximumAttendeesInvalid");
        _TitleInvalid = new LocalizedString("TitleInvalid", "TitleInvalid");
        _DetailsInvalid = new LocalizedString("DetailsInvalid", "DetailsInvalid");
        _TitleUsed = new LocalizedString("TitleUsed", "TitleUsed");

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

        _repository = new EventRepository(_dbContext, _mapper.Object, _mockStringLocalizer.Object);

        _eventId = Guid.NewGuid();
        _attendeeId = Guid.NewGuid();

        //var eventEntity = new Event() { Id = _eventId, Maximum_Attendees = 2 };

        //_dbContext.Events.Add(eventEntity);
        //_dbContext.SaveChanges();
    }

    [Fact]
    public void CreateNewEvent_ValidRequest_ShouldReturn_ResponseRegisteredJson()
    {
        // Arrange
        var request = new RequestEventJson()
        {
            Title = "Title",
            Details = "Details",
            Maximum_Attendees = 1
        };

        // Act
        var result = _repository.CreateNewEvent(request);

        // Assert
        Assert.NotNull(result);
        result.Id.Should().NotBeEmpty();
        Assert.IsType<ResponseRegisteredJson>(result);
        Assert.IsType<Guid>(result.Id);
    }

    [Theory]
    [InlineData("Title", "Details", 0)]
    [InlineData("Title", "", 1)]
    [InlineData("", "Details", 1)]
    public void CreateNewEvent_ValidRequest_ShouldReturnThrow_ErrorOnValidationException(string title, string details, int maximumAttendees)
    {
        // Arrange
        var request = new RequestEventJson()
        {
            Title = title,
            Details = details,
            Maximum_Attendees = maximumAttendees
        };

        // Act & Assert
        Assert.Throws<ErrorOnValidationException>(() => _repository.CreateNewEvent(request));
    }

    [Fact]
    public void GetEventById_ValidRequest_ShouldReturn_ResponseRegisteredJson()
    {
        // Arrange
        var eventEntity = new Event() { Id = Guid.NewGuid(), Maximum_Attendees = 2 };

        _dbContext.Events.Add(eventEntity);
        _dbContext.SaveChanges();

        // Act
        var result = _repository.GetEventById(eventEntity.Id);

        // Assert
        Assert.NotNull(result);
        result.Id.Should().NotBeEmpty();
        Assert.IsType<ResponseEventJson>(result);
        Assert.IsType<Guid>(result.Id);
    }

    [Fact]
    public void GetEventById_ValidRequest_ShouldReturnThrow_NotFoundException()
    {
        // Act & Assert
        Assert.Throws<NotFoundException>(() => _repository.GetEventById(Guid.NewGuid()));
    }
}