using FluentAssertions;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Services;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Services;

public class InnoGotchiSignsUpdateServiceTests
{
    private static readonly DateTime Now = DateTime.MinValue.AddDays(10);
    private static readonly TimeSpan TimeToIncreaseAge = TimeSpan.FromDays(1);
    private static readonly TimeSpan TimeToIncreaseHungerLevel = TimeSpan.FromDays(1);
    private static readonly TimeSpan TimeToIncreaseThirstLevel = TimeSpan.FromDays(1);
    private readonly InnoGotchiSignsUpdateService _sut;
    public InnoGotchiSignsUpdateServiceTests()
    {
        var timeServiceMock = new Mock<ITimeService>();
        timeServiceMock.Setup(x => x.Now()).Returns(Now);

        _sut = new InnoGotchiSignsUpdateService(timeServiceMock.Object,
            TimeToIncreaseHungerLevel, TimeToIncreaseThirstLevel, TimeToIncreaseAge);
    }

    [Fact]
    public void TryIncreaseHungerLevel_ShouldNotIncreaseBecauseTimeFromLastFeedingLessThanTimeToIncreaseHungerLevel_Test()
    {
        var lastFeedTime = Now.Subtract(TimeToIncreaseHungerLevel - TimeSpan.FromMilliseconds(100));
        const HungerLevel currentHungerLevel = HungerLevel.Full;

        var newHungerLevel = _sut.TryIncreaseHungerLevel(currentHungerLevel, lastFeedTime);

        newHungerLevel.Should().Be(currentHungerLevel);
    }

    [Fact]
    public void TryIncreaseThirstLevel_ShouldIncreaseBecauseTimeFromLastFeedingGreaterThanTimeToIncreaseThirstLevel_Test()
    {
        var lastDrinkTime = Now.Subtract(TimeToIncreaseThirstLevel);
        const ThirstyLevel currentThirstLevel = ThirstyLevel.Full;

        var newHungerLevel = _sut.TryIncreaseThirstLevel(currentThirstLevel, lastDrinkTime);

        newHungerLevel.Should().Be(ThirstyLevel.Normal);
    }

    [Fact]
    public void TryIncreaseAge_ShouldBeExpected_Test()
    {
        const int expected = 6;
        var lastAgeUpdateTime = Now.Subtract(expected * TimeToIncreaseAge);

        var actual = _sut.TryIncreaseAge(0, lastAgeUpdateTime);

        actual.Should().Be(expected);
    }

    [Fact]
    public void IsDeadNow_ShouldReturnTrueBecauseHungerTypeEqualToDead_Test()
    {
        const HungerLevel hungerLevel = HungerLevel.Dead;
        const ThirstyLevel thirstLevel = ThirstyLevel.Full;

        var actual = _sut.IsDeadNow(hungerLevel, thirstLevel, 0);

        actual.Should().BeTrue();
    }

    [Fact]
    public void IsDeadNow_ShouldReturnFalse_Test()
    {
        const HungerLevel hungerLevel = HungerLevel.Hungry;
        const ThirstyLevel thirstLevel = ThirstyLevel.Full;

        var actual = _sut.IsDeadNow(hungerLevel, thirstLevel, 0);

        actual.Should().BeFalse();
    }

    [Fact]
    public void CalculateHappinessDaysCount_ShouldReturnZeroBecauseThirstLevelIsThirsty_Test()
    {
        var days = 5;
        const HungerLevel hungerLevel = HungerLevel.Full;
        const ThirstyLevel thirstLevel = ThirstyLevel.Thirsty;
        var petCreationDate = Now.Subtract(TimeSpan.FromDays(days));

        var actual = _sut.CalculateHappinessDaysCount(hungerLevel, thirstLevel, petCreationDate);

        actual.Should().Be(0);
    }

    [Fact]
    public void CalculateHappinessDaysCount_ShouldReturnDaysCount_Test()
    {
        const int days = 5;
        const HungerLevel hungerLevel = HungerLevel.Full;
        const ThirstyLevel thirstLevel = ThirstyLevel.Normal;
        var petCreationDate = Now.Subtract(TimeSpan.FromDays(days));

        var actual = _sut.CalculateHappinessDaysCount(hungerLevel, thirstLevel, petCreationDate);

        actual.Should().Be(days);
    }
}