using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Services.Services;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Data;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.PagedList;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.InnoGotchies;

public class GetInnoGotchiPagedListQueryHandlerTests
{
    private readonly GetInnoGotchiPagedListQueryHandler _sut;
    private readonly Guid _profileId;
    private readonly DomainData _domainData;
    public GetInnoGotchiPagedListQueryHandlerTests()
    {
        var data = UnitOfWorkMock.Create();
        var mapper = DomainMocks.DomainMapper();
        var sortingService = new SortingServiceBase<InnoGotchiModel>(
            InnoGotchiSortingExpressions.GetSortingExpressions());

        _domainData = data.Data;
        _profileId = data.Profiles.Last().Id;
        _sut = new GetInnoGotchiPagedListQueryHandler(data.UnitOfWork, mapper, sortingService);
    }

    [Fact]
    public async Task ShouldReturnDetailedInnoGotchi_Test()
    {
        var pagedListData = new PagedListData
        {
            IsAscending = false,
            PageSize = 10,
            PageIndex = 0,
            SortType = 0
        };

        var query = new GetInnoGotchiPagedListQuery(_profileId, pagedListData);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result!.PageSize.Should().Be(pagedListData.PageSize);
        result.Result.PageIndex.Should().Be(pagedListData.PageIndex);
        result.Result.TotalCount.Should().Be(_domainData.GetPetsForFirstProfile().Count);
    }
}