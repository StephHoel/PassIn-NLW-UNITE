using Bogus;
using Domain.Entities;

namespace UnitTests.FakeObjects;

public class FakeEvent
{
    public static Event Generate(List<Attendee> attendeeList)
    {
        return new Faker<Event>()
            .RuleFor(c => c.Title, f => f.Random.String())
            .RuleFor(c => c.Details, f => f.Random.String())
            .RuleFor(c => c.Maximum_Attendees, f => f.Random.Int(0, 10))
            .RuleFor(c => c.Attendees, attendeeList);
    }
}