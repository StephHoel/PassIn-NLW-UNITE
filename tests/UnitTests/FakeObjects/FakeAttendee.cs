using Bogus;
using Domain.Entities;

namespace UnitTests.FakeObjects;

public class FakeAttendee
{
    public static Attendee Generate(CheckIn? checkIn)
    {
        return new Faker<Attendee>()
            .RuleFor(a => a.Name, f => f.Random.String())
            .RuleFor(a => a.Email, f => f.Random.String())
            .RuleFor(a => a.Event_Id, f => f.Random.Guid())
            .RuleFor(a => a.Created_At, f => f.Date.Recent())
            .RuleFor(a => a.CheckIn, _ => checkIn);
    }

    public static Attendee Generate(Guid eventId, CheckIn? checkIn)
    {
        return new Faker<Attendee>()
            .RuleFor(a => a.Name, f => f.Random.String())
            .RuleFor(a => a.Email, f => f.Random.String())
            .RuleFor(a => a.Event_Id, eventId)
            .RuleFor(a => a.Created_At, f => f.Date.Recent())
            .RuleFor(a => a.CheckIn, _ => checkIn);
    }

    public static Attendee Generate(Guid eventId, Guid attendeeId, CheckIn? checkIn)
    {
        return new Faker<Attendee>()
            .RuleFor(a => a.Id, attendeeId)
            .RuleFor(a => a.Name, f => f.Random.String())
            .RuleFor(a => a.Email, f => f.Random.String())
            .RuleFor(a => a.Event_Id, eventId)
            .RuleFor(a => a.Created_At, f => f.Date.Recent())
            .RuleFor(a => a.CheckIn, _ => checkIn);
    }
}