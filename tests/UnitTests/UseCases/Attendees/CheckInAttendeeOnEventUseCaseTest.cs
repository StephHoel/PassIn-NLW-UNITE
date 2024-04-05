using Application.UseCases.Attendees;
using Bogus;
using Communication.Responses;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.UseCases.Attendees;

public class CheckInAttendeeOnEventUseCaseTest
{
    [Fact]
    public void Sucess()
    {
        //ARRANGE
        var attendeeId = Guid.NewGuid();

        var entity = NewEntity();

        var mock = new Mock<ICheckInRepository>();
        mock.Setup(i => i.DoCheckIn(attendeeId)).Returns(entity);

        var useCase = new CheckInAttendeeOnEventUseCase(mock.Object);

        //ACT
        var checkIn = useCase.Execute(attendeeId);

        //ASSERT
        checkIn.Result.Should().NotBeNull();
        checkIn.Result.Id.Should().Be(entity.Id);
    }

    [Fact]
    public void ErrorCheckInBeNull()
    {
        //ARRANGE
        var attendeeId = Guid.NewGuid();
        var id = Guid.NewGuid();

        var entity = NewEntity();

        var mock = new Mock<ICheckInRepository>();
        mock.Setup(i => i.DoCheckIn(id)).Returns(entity);

        var useCase = new CheckInAttendeeOnEventUseCase(mock.Object);

        //ACT
        var checkIn = useCase.Execute(attendeeId);

        //ASSERT
        checkIn.Result.Should().BeNull();
    }

    private ResponseRegisteredJson NewEntity()
    {
        return new Faker<ResponseRegisteredJson>()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .Generate();
    }
}