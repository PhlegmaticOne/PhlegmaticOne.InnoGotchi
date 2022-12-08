using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.Farms;

public class GetFarmByIdQueryTests
{
    private readonly UnitOfWorkMock _data;
    private readonly GetFarmByIdQueryHandler _sut;
    public GetFarmByIdQueryTests()
    {
        _data = UnitOfWorkMock.Create();

        var mapper = new MapperConfiguration(x =>
        {
            x.AddProfile(new FarmMapperConfiguration());
            x.AddProfile(new InnoGotchiComponentsMapperConfiguration());
            x.AddProfile(new InnoGotchiesMapperConfiguration());
        }).CreateMapper();

        var validator = new Mock<IValidator<GetFarmByIdQuery>>();
        validator.Setup(x => x.ValidateAsync(It.IsAny<GetFarmByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new GetFarmByIdQueryHandler(_data.UnitOfWork, validator.Object, mapper);
    }

    [Fact]
    public async Task ShouldReturnFarm_Test()
    {
        var profile = _data.ThatHasFarm;
        var farmId = profile.Farm!.Id;
        var profileId = profile.Id;
        var query = new GetFarmByIdQuery(profileId, farmId);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result!.Email.Should().Be(profile.User.Email);
        result.Result.FirstName.Should().Be(profile.FirstName);
        result.Result.LastName.Should().Be(profile.LastName);
        result.Result.InnoGotchies.Should().HaveCount(profile.Farm.InnoGotchies.Count);
    }
}