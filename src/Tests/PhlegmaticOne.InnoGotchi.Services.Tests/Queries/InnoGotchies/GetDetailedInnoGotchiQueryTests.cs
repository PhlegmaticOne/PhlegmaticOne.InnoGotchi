using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Data;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.InnoGotchies;

public class GetDetailedInnoGotchiQueryTests
{
    private readonly InnoGotchiModel _model;
    private readonly GetDetailedInnoGotchiQueryHandler _sut;
    public GetDetailedInnoGotchiQueryTests()
    {
        var data = UnitOfWorkMock.Create();
        _model = data.Data.GetPetsForFirstProfile().First();
        var readableProvider = DomainMocks.ReadablePetProviderWithPetToReturn(_model);
        var mapper = DomainMocks.DomainMapper();
        var validator = DomainMocks.AlwaysTrueValidator<GetDetailedInnoGotchiQuery>();
        _sut = new GetDetailedInnoGotchiQueryHandler(readableProvider, mapper, validator);
    }

    [Fact]
    public async Task ShouldReturnDetailedInnoGotchi_Test()
    {
        var query = new GetDetailedInnoGotchiQuery(_model.Farm.OwnerId, _model.Id);
        var result = await _sut.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result!.Name.Should().Be(_model.Name);
        result.Result.Components
            .All(x => _model.Components.Any(c => c.InnoGotchiComponent.Name == x.Name &&
                                                 c.InnoGotchiComponent.ImageUrl == x.ImageUrl))
            .Should().BeTrue();
    }
}