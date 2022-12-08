using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Services.Queries.Statistics;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Data;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.Statistics;

public class GetDetailedStatisticsQueryHandlerTests
{
    private readonly GetDetailedStatisticsQueryHandler _sut;
    private UserProfile _model;
    private readonly DomainData _domainData;

    public GetDetailedStatisticsQueryHandlerTests()
    {
        var data = UnitOfWorkMock.Create();
        var validator = DomainMocks.AlwaysTrueValidator<ProfileFarmModel>();
        _domainData = data.Data;
        _model = data.Profiles.First();
        _sut = new GetDetailedStatisticsQueryHandler(data.UnitOfWork, validator);
    }

    [Fact]
    public async Task ShouldBuildStatistics_Test()
    {
        var profileId = _model.Id;
        var query = new GetDetailedStatisticsQuery(profileId);

        var result = await _sut.Handle(query, CancellationToken.None);

        var pets = _domainData.GetPetsForFirstProfile();
        var farmStatistics = pets.First().Farm.FarmStatistics;

        result.IsSuccess.Should().BeTrue();
        result.Result!.AlivePetsCount.Should().Be(pets.Count(x => x.IsDead == false));
        result.Result.AverageAlivePetsAge.Should().Be(pets.Where(x => x.IsDead == false).Average(x => x.Age));
        result.Result.AverageFeedingPeriod.Should().Be(farmStatistics.AverageFeedTime);
        result.Result.AverageThirstQuenchingPeriod.Should().Be(farmStatistics.AverageDrinkTime);
    }
}