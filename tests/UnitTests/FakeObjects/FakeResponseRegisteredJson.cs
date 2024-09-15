using Bogus;
using Communication.Responses;

namespace UnitTests.FakeObjects;

public class FakeResponseRegisteredJson
{
    public static ResponseRegisteredJson Generate()
    {
        return new Faker<ResponseRegisteredJson>()
                .RuleFor(a => a.Id, f => f.Random.Guid());
    }
}