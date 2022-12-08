using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Services.Queries.Statistics;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Data;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.Statistics;

public class GetGlobalStatisticsQueryHandlerTests
{
    private readonly GetGlobalStatisticsQueryHandler _sut;
    private readonly DomainData _domainData;
    public GetGlobalStatisticsQueryHandlerTests()
    {
        var data = UnitOfWorkMock.Create();
        _domainData = data.Data;
        _sut = new GetGlobalStatisticsQueryHandler(data.UnitOfWork);
    }

    [Fact]
    public async Task ShouldBuildStatistics_Test()
    {
        var query = new GetGlobalStatisticsQuery();

        var result = await _sut.Handle(query, CancellationToken.None);

        var profiles = _domainData.GetProfiles();
        result.IsSuccess.Should().BeTrue();
        result.Result!.CollaborationsCount.Should().Be(1);
        result.Result.DeadPetsCount.Should().Be(1);
        result.Result.AlivePetsCount.Should().Be(1);
        result.Result.PetsTotalCount.Should().Be(2);
        result.Result.FarmsCount.Should().Be(profiles.Count(x => x.Farm is not null));
        result.Result.ProfilesCount.Should().Be(profiles.Count);
    }
}