using FluentAssertions;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Exceptions;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Services.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Tests.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Providers.Writable;

public class WritableFarmStatisticsProviderTests
{
    private readonly UnitOfWorkData _data;
    private static readonly DateTime Now = DateTime.MinValue.AddDays(10);
    public WritableFarmStatisticsProviderTests() => _data = UnitOfWorkMock.Create();

    [Fact]
    public async Task ProcessFeeding_ShouldThrowExceptionBecauseProfileDoesNotHaveStatistics_Test()
    {
        var profileId = _data.ThatHasNoFarm.Id;

        var timeService = new Mock<ITimeService>();
        timeService.Setup(x => x.Now()).Returns(Now);

        var sut = new WritableFarmStatisticsProvider(_data.UnitOfWork, timeService.Object);

        await sut.Invoking(x => x.ProcessFeedingAsync(profileId))
            .Should()
            .ThrowAsync<DomainException>()
            .WithMessage(AppErrorMessages.ProfileDoesNotHaveFarmStatistics);
    }

    [Fact]
    public async Task ProcessFeeding_ShouldUpdateStatistics_Test()
    {
        var profileId = _data.ThatHasFarm.Id;
        var lastFeedTime = _data.ThatHasFarm.Farm!.FarmStatistics.LastFeedTime;
        var totalFeedings = _data.ThatHasFarm.Farm!.FarmStatistics.TotalFeedingsCount;
        var currentAverage = _data.ThatHasFarm.Farm!.FarmStatistics.AverageFeedTime;
        var now = Now;

        var timeService = new Mock<ITimeService>();
        timeService.Setup(x => x.Now()).Returns(Now);

        var sut = new WritableFarmStatisticsProvider(_data.UnitOfWork, timeService.Object);

        var feedingResult = await sut.ProcessFeedingAsync(profileId);

        feedingResult.AverageFeedTime.Should()
            .Be(AverageCalculator.CalculateNewAverage(currentAverage, lastFeedTime, now, totalFeedings));
        feedingResult.TotalFeedingsCount.Should().Be(totalFeedings + 1);
        feedingResult.LastFeedTime.Should().Be(Now);
    }

    [Fact]
    public async Task ProcessDrinking_ShouldUpdateStatistics_Test()
    {
        var profileId = _data.ThatHasFarm.Id;
        var lastDrinkTime = _data.ThatHasFarm.Farm!.FarmStatistics.LastDrinkTime;
        var totalDrinkings = _data.ThatHasFarm.Farm!.FarmStatistics.TotalDrinkingsCount;
        var currentAverage = _data.ThatHasFarm.Farm!.FarmStatistics.AverageDrinkTime;
        var now = Now;

        var timeService = new Mock<ITimeService>();
        timeService.Setup(x => x.Now()).Returns(Now);

        var sut = new WritableFarmStatisticsProvider(_data.UnitOfWork, timeService.Object);

        var feedingResult = await sut.ProcessDrinkingAsync(profileId);

        feedingResult.AverageDrinkTime.Should()
            .Be(AverageCalculator.CalculateNewAverage(currentAverage, lastDrinkTime, now, totalDrinkings));
        feedingResult.TotalDrinkingsCount.Should().Be(totalDrinkings + 1);
        feedingResult.LastDrinkTime.Should().Be(Now);
    }
}