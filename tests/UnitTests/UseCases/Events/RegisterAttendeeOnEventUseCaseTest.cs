using Application.UseCases.Events;
using Bogus;
using Communication.Requests;
using Communication.Responses;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.UseCases.Events;

public class RegisterAttendeeOnEventUseCaseTest
{
    [Fact]
    public void Sucess()
    {
        //ARRANGE
        var eventId = Guid.NewGuid();
        var request = NewRequest();

        var entity = NewEntity();

        var mock = new Mock<IAttendeeRepository>();
        mock.Setup(i => i.CreateNewAttendee(eventId, request)).Returns(entity);

        var useCase = new RegisterAttendeeOnEventUseCase(mock.Object);

        //ACT
        var attendeeUseCase = useCase.Execute(eventId, request);

        //ASSERT
        attendeeUseCase.Result.Should().NotBeNull();
        attendeeUseCase.Result.Id.Should().Be(entity.Id);
    }

    [Fact]
    public void ErrorAttendeeBeNull()
    {
        //ARRANGE
        var eventId = Guid.NewGuid();
        var newId = Guid.NewGuid();
        var request = NewRequest();

        var entity = NewEntity();

        var mock = new Mock<IAttendeeRepository>();
        mock.Setup(i => i.CreateNewAttendee(eventId, request)).Returns(entity);

        var useCase = new RegisterAttendeeOnEventUseCase(mock.Object);

        //ACT
        var attendeeUseCase = useCase.Execute(newId, request);

        //ASSERT
        attendeeUseCase.Result.Should().BeNull();
    }

    private RequestRegisterEventJson NewRequest()
    {
        return new Faker<RequestRegisterEventJson>()
            .RuleFor(a => a.Name, f => f.Random.String())
            .RuleFor(a => a.Email, f => f.Internet.Email())
            .Generate();
    }

    private ResponseRegisteredJson NewEntity()
    {
        return new Faker<ResponseRegisteredJson>()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .Generate();
    }
}