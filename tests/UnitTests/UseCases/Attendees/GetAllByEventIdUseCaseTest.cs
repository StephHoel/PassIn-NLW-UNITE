using Application.UseCases.Attendees;
using Bogus;
using Communication.Responses;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.UseCases.Attendees;

public class GetAllByEventIdUseCaseTest
{
    [Fact]
    public void Sucess()
    {
        //ARRANGE
        var eventId = Guid.NewGuid();

        var response = NewEntity();

        var mock = new Mock<IAttendeeRepository>();
        mock.Setup(i => i.GetAllByEventId(eventId)).Returns(response);

        var useCase = new GetAllByEventIdUseCase(mock.Object);

        //ACT
        var eventUseCase = useCase.Execute(eventId);

        //ASSERT
        eventUseCase.Should().NotBeNull();
        eventUseCase.Attendees.Should().NotBeNull();
        eventUseCase.Attendees.Should().BeSameAs(response.Attendees);
    }

    [Fact]
    public void ErrorUseCaseShouldBeNull()
    {
        //ARRANGE
        var eventId = Guid.NewGuid();
        var id = Guid.NewGuid();

        var response = NewEntity();

        var mock = new Mock<IAttendeeRepository>();
        mock.Setup(i => i.GetAllByEventId(id)).Returns(response);

        var useCase = new GetAllByEventIdUseCase(mock.Object);

        //ACT
        var eventUseCase = useCase.Execute(eventId);

        //ASSERT
        eventUseCase.Should().BeNull();
    }

    [Fact]
    public void ErrorUseCaseShouldBeNotNullAndAttendeesShouldBeEmpty()
    {
        //ARRANGE
        var eventId = Guid.NewGuid();
        var id = Guid.NewGuid();

        var response
            = new Faker<ResponseAllAttendeesJson>()
            .RuleFor(a => a.Attendees, f => [])
            .Generate();

        var mock = new Mock<IAttendeeRepository>();
        mock.Setup(i => i.GetAllByEventId(eventId)).Returns(response);

        var useCase = new GetAllByEventIdUseCase(mock.Object);

        //ACT
        var eventUseCase = useCase.Execute(eventId);

        //ASSERT
        eventUseCase.Should().NotBeNull();
        eventUseCase.Attendees.Should().BeEmpty();
    }

    private ResponseAllAttendeesJson NewEntity()
    {
        return new Faker<ResponseAllAttendeesJson>()
            .RuleFor(a => a.Attendees, f => [ new ResponseAttendeeJson()
            {
                Id = f.Random.Guid(),
                Name = f.Random.String(),
                Email = f.Internet.Email(),
                CheckedIn_At = f.Date.Future(),
                Created_At= f.Date.Past()
            } ])
            .Generate();
    }
}