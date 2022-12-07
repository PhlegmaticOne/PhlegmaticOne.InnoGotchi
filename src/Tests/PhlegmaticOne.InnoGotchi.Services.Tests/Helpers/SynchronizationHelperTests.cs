using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Helpers;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Helpers;

public class SynchronizationHelperTests
{
    [Fact]
    public void IncreaseUntilNotSynchronizedWithTime_ShouldNotIncreaseValueBecauseTimeSinceLastActionLessThanPeriod_Test()
    {
        var expected = 42;
        var lastActionTime = DateTime.MinValue;
        var now = DateTime.MinValue.AddDays(1);
        var actionTime = TimeSpan.FromDays(2);

        var actual =
            SynchronizationHelper.IncreaseUntilNotSynchronizedWithTime(expected, now, lastActionTime, actionTime);

        expected.Should().Be(actual);
    }

    [Fact]
    public void IncreaseUntilNotSynchronizedWithTime_ShouldIncreaseOnceBecauseTimeSinceLastActionGreaterThanOnePeriod_Test()
    {
        var value = 42;
        var lastActionTime = DateTime.MinValue;
        var now = DateTime.MinValue.AddDays(3);
        var actionTime = TimeSpan.FromDays(2);

        var actual =
            SynchronizationHelper.IncreaseUntilNotSynchronizedWithTime(value, now, lastActionTime, actionTime);

        actual.Should().Be(value + 1);
    }
}