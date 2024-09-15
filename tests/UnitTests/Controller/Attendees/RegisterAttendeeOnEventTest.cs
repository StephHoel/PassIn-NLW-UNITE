using Api.Controllers;
using Application.UseCases.Events;
using Communication.Requests;
using Communication.Responses;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitTests.FakeObjects;
using Xunit;

namespace UnitTests.Controller.Attendees;

public class RegisterAttendeeOnEventTest
{
    private readonly RegisterAttendeeOnEventUseCase _useCase;

    public RegisterAttendeeOnEventTest()
    {
        var iAttendeeRepository = new Mock<IAttendeeRepository>();
        _useCase = new RegisterAttendeeOnEventUseCase(iAttendeeRepository.Object);
    }

    [Fact]
    public void RegisterAttendeeOnEvent_ValidRequest_ShouldReturn_CreatedResult()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var request = FakeRequestRegisterEventJson.Generate();
        var response = FakeResponseRegisteredJson.Generate();


        var controller = new AttendeesController();

        // Act
        var result = controller.RegisterAttendeeOnEvent(_useCase, eventId, request);


        // Assert
        result.Should().BeOfType<CreatedResult>();
    }
}
