using Domain.Entities;
using UnitTests.FakeObjects;
using Xunit;

namespace UnitTests.Entity;

public class CheckInTest
{
    [Fact]
    public void CheckInShouldBeNotNull()
    {
        var checkin = FakeCheckIn.Generate();

        Assert.NotNull(checkin);
        Assert.IsType<CheckIn>(checkin);

        Assert.IsType<Guid>(checkin.Id);
        Assert.IsType<DateTime>(checkin.Created_at);
        Assert.IsType<Guid>(checkin.Attendee_Id);
    }
}