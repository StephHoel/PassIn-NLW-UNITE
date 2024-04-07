using Domain.Entities;
using UnitTests.FakeObjects;
using Xunit;

namespace UnitTests.Entity;

public class AttendeeTest
{
    [Fact]
    public void SucessWithCheckInNotNull()
    {
        var checkIn = FakeCheckIn.Generate();
        var attendee = FakeAttendee.Generate(checkIn);

        Assert.NotNull(attendee);
        Assert.IsType<Attendee>(attendee);
        Assert.NotNull(attendee.CheckIn);
        Assert.IsType<CheckIn>(attendee.CheckIn);

        Assert.IsType<Guid>(attendee.Id);
        Assert.IsType<string>(attendee.Name);
        Assert.IsType<string>(attendee.Email);
        Assert.IsType<Guid>(attendee.Event_Id);
        Assert.IsType<DateTime>(attendee.Created_At);

        Assert.IsType<Guid>(attendee.CheckIn.Id);
        Assert.IsType<DateTime>(attendee.CheckIn.Created_at);
        Assert.IsType<Guid>(attendee.CheckIn.Attendee_Id);
    }

    [Fact]
    public void SucessWithCheckInNull()
    {
        var attendee = FakeAttendee.Generate(null);

        Assert.NotNull(attendee);
        Assert.IsType<Attendee>(attendee);
        Assert.Null(attendee.CheckIn);
    }
}