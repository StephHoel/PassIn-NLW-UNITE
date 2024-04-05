using Application.UseCases.Events;
using Bogus;
using Communication.Requests;
using Communication.Responses;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.UseCases.Events;

public class RegisterEventUseCaseTest
{
    [Fact]
    public void Sucess()
    {
        //ARRANGE
        var request = NewRequest();

        var entity = NewEntity();

        var mock = new Mock<IEventRepository>();
        mock.Setup(i => i.CreateNewEvent(request)).Returns(entity);

        var useCase = new RegisterEventUseCase(mock.Object);

        //ACT
        var eventUseCase = useCase.Execute(request);

        //ASSERT
        eventUseCase.Result.Should().NotBeNull();
        eventUseCase.Result.Id.Should().Be(entity.Id);
    }

    [Fact]
    public void ErrorEventNotCreated()
    {
        //ARRANGE
        var request = NewRequest();
        var newRequest = NewRequest();

        var entity = NewEntity();

        var mock = new Mock<IEventRepository>();
        mock.Setup(i => i.CreateNewEvent(request)).Returns(entity);

        var useCase = new RegisterEventUseCase(mock.Object);

        //ACT
        var eventUseCase = useCase.Execute(newRequest);

        //ASSERT
        eventUseCase.Result.Should().BeNull();
    }

    private RequestEventJson NewRequest()
    {
        return new Faker<RequestEventJson>()
            .RuleFor(a => a.Title, f => f.Random.String())
            .RuleFor(a => a.Details, f => f.Random.String())
            .RuleFor(a => a.Maximum_Attendees, f => f.Random.Int(0,10))
            .Generate();
    }

    private ResponseRegisteredJson NewEntity()
    {
        return new Faker<ResponseRegisteredJson>()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .Generate();
    }
}