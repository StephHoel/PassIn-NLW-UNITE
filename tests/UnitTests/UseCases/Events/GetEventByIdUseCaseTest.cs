using Application.UseCases.Attendees;
using Application.UseCases.Events;
using Bogus;
using Communication.Responses;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.UseCases.Events;

public class GetEventByIdUseCaseTest
{
    [Fact]
    public void Sucess()
    {
        //ARRANGE
        var id = Guid.NewGuid();

        var entity = NewEntity(id);

        var mock = new Mock<IEventRepository>();
        mock.Setup(i => i.GetEventById(id)).Returns(entity);

        var useCase = new GetEventByIdUseCase(mock.Object);

        //ACT
        var eventUseCase = useCase.Execute(id);

        //ASSERT
        eventUseCase.Result.Should().NotBeNull();
        eventUseCase.Result.Id.Should().Be(entity.Id);
        eventUseCase.Result.Title.Should().Be(entity.Title);
        eventUseCase.Result.Details.Should().Be(entity.Details);
        eventUseCase.Result.Maximum_Attendees.Should().Be(entity.Maximum_Attendees);
        eventUseCase.Result.Attendees_Amount.Should().Be(entity.Attendees_Amount);
    }

    [Fact]
    public void ErrorEventBeNull()
    {
        //ARRANGE
        var id = Guid.NewGuid();
        var newId = Guid.NewGuid();

        var entity = NewEntity(id);

        var mock = new Mock<IEventRepository>();
        mock.Setup(i => i.GetEventById(id)).Returns(entity);

        var useCase = new GetEventByIdUseCase(mock.Object);

        //ACT
        var eventUseCase = useCase.Execute(newId);

        //ASSERT
        eventUseCase.Result.Should().BeNull();
    }

    private ResponseEventJson NewEntity(Guid id)
    {
        return new Faker<ResponseEventJson>()
            .RuleFor(a => a.Id, id)
            .RuleFor(a => a.Title, f => f.Random.String())
            .RuleFor(a => a.Details, f => f.Random.String())
            .RuleFor(a => a.Maximum_Attendees, f => f.Random.Int(1, 10))
            .RuleFor(a => a.Attendees_Amount, f => f.Random.Int(0, 10))
            .Generate();
    }
}