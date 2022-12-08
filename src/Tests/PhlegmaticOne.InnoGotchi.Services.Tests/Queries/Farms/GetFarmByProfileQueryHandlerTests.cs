using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.Farms;

public class GetFarmByProfileQueryHandlerTests
{
    private readonly UnitOfWorkMock _data;
    private readonly GetFarmByProfileQueryHandler _sut;
    public GetFarmByProfileQueryHandlerTests()
    {
        _data = UnitOfWorkMock.Create();

        var mapper = new MapperConfiguration(x =>
        {
            x.AddProfile(new FarmMapperConfiguration());
            x.AddProfile(new InnoGotchiComponentsMapperConfiguration());
            x.AddProfile(new InnoGotchiesMapperConfiguration());
        }).CreateMapper();

        var validator = new Mock<IValidator<ProfileFarmModel>>();
        validator.Setup(x => x.ValidateAsync(It.IsAny<ProfileFarmModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _sut = new GetFarmByProfileQueryHandler(_data.UnitOfWork, mapper, validator.Object);
    }
    [Fact]
    public async Task ShouldReturnFarm_Test()
    {
        var profile = _data.ThatHasFarm;
        var profileId = profile.Id;
        var query = new GetFarmByProfileQuery(profileId);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result!.Email.Should().Be(profile.User.Email);
        result.Result.FirstName.Should().Be(profile.FirstName);
        result.Result.LastName.Should().Be(profile.LastName);
        result.Result.InnoGotchies.Should().HaveCount(profile.Farm.InnoGotchies.Count);
    }
}