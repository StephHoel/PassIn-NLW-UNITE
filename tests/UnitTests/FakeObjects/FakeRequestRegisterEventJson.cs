using Bogus;
using Communication.Requests;

namespace UnitTests.FakeObjects;

public class FakeRequestRegisterEventJson
{
    public static RequestRegisterEventJson Generate()
    {
        return new Faker<RequestRegisterEventJson>()
               .RuleFor(a => a.Name, f => f.Random.String())
               .RuleFor(a => a.Email, f => f.Internet.Email());
    }
}