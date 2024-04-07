using Domain.Entities;
using UnitTests.FakeObjects;
using Xunit;

namespace UnitTests.Entity;

public class EventTest
{
    [Fact]
    public void Sucess()
    {
        var checkIn = FakeCheckIn.Generate();
        var attendee = FakeAttendee.Generate(checkIn);
        var eventFake = FakeEvent.Generate([attendee]);

        Assert.NotNull(eventFake);
        Assert.IsType<Event>(eventFake);

        Assert.IsType<Guid>(eventFake.Id);
        Assert.IsType<string>(eventFake.Title);
        Assert.IsType<string>(eventFake.Details);
        Assert.IsType<string>(eventFake.Slug);
        Assert.IsType<int>(eventFake.Maximum_Attendees);

        Assert.NotNull(eventFake.Attendees);
        Assert.IsType<List<Attendee>>(eventFake.Attendees);
        Assert.IsType<Attendee>(eventFake.Attendees[0]);

        Assert.IsType<Guid>(eventFake.Attendees[0].Id);
        Assert.IsType<string>(eventFake.Attendees[0].Name);
        Assert.IsType<string>(eventFake.Attendees[0].Email);
        Assert.IsType<Guid>(eventFake.Attendees[0].Event_Id);
        Assert.IsType<DateTime>(eventFake.Attendees[0].Created_At);

        Assert.NotNull(eventFake.Attendees[0].CheckIn);
        Assert.IsType<CheckIn>(eventFake.Attendees[0].CheckIn);

        Assert.IsType<Guid>(eventFake.Attendees[0].CheckIn!.Id);
        Assert.IsType<DateTime>(eventFake.Attendees[0].CheckIn!.Created_at);
        Assert.IsType<Guid>(eventFake.Attendees[0].CheckIn!.Attendee_Id);
    }

    [Fact]
    public void SucessWithAttendeeListEmpty()
    {
        var eventFake = FakeEvent.Generate([]);

        Assert.NotNull(eventFake);
        Assert.IsType<Event>(eventFake);

        Assert.IsType<Guid>(eventFake.Id);
        Assert.IsType<string>(eventFake.Title);
        Assert.IsType<string>(eventFake.Details);
        Assert.IsType<string>(eventFake.Slug);
        Assert.IsType<int>(eventFake.Maximum_Attendees);

        Assert.Equal(eventFake.Attendees, []);
    }

    [Fact]
    public void SucessWithAttendeeListNotEmptyAndCheckInNull()
    {
        var attendee = FakeAttendee.Generate(null);
        var eventFake = FakeEvent.Generate([attendee]);

        Assert.NotNull(eventFake);
        Assert.IsType<Event>(eventFake);

        Assert.IsType<Guid>(eventFake.Id);
        Assert.IsType<string>(eventFake.Title);
        Assert.IsType<string>(eventFake.Details);
        Assert.IsType<string>(eventFake.Slug);
        Assert.IsType<int>(eventFake.Maximum_Attendees);

        Assert.NotNull(eventFake.Attendees);
        Assert.IsType<List<Attendee>>(eventFake.Attendees);
        Assert.IsType<Attendee>(eventFake.Attendees[0]);

        Assert.IsType<Guid>(eventFake.Attendees[0].Id);
        Assert.IsType<string>(eventFake.Attendees[0].Name);
        Assert.IsType<string>(eventFake.Attendees[0].Email);
        Assert.IsType<Guid>(eventFake.Attendees[0].Event_Id);
        Assert.IsType<DateTime>(eventFake.Attendees[0].Created_At);

        Assert.Null(eventFake.Attendees[0].CheckIn);
    }
}