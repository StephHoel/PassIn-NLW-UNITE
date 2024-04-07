using Bogus;
using Domain.Entities;

namespace UnitTests.FakeObjects;

public class FakeCheckIn
{
    public static CheckIn Generate()
    {
        return new Faker<CheckIn>()
            .RuleFor(c => c.Created_at, f => f.Date.Past())
            .RuleFor(c => c.Attendee_Id, f => f.Random.Guid());
    }

    public static CheckIn Generate(Guid attendeeId)
    {
        return new Faker<CheckIn>()
            .RuleFor(c => c.Created_at, f => f.Date.Past())
            .RuleFor(c => c.Attendee_Id, attendeeId);
    }
}