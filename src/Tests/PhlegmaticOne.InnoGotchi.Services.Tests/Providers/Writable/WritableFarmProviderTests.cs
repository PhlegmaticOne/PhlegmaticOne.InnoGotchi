using FluentAssertions;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Exceptions;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Tests.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Providers.Writable;


public class WritableFarmProviderTests
{
    private readonly UnitOfWorkData _data;
    private static readonly DateTime Now = DateTime.MinValue;
    public WritableFarmProviderTests() => _data = UnitOfWorkMock.Create();

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionBecauseFromProfileDoesNotExist_Test()
    {
        var profileId = Guid.Empty;
        var dto = new CreateFarmDto
        {
            Name = "Farm"
        };

        var timeService = new Mock<ITimeService>();
        timeService.Setup(x => x.Now()).Returns(Now);

        var sut = new WritableFarmProvider(_data.UnitOfWork, timeService.Object);

        await sut.Invoking(x => x.CreateAsync(profileId, dto))
            .Should()
            .ThrowAsync<DomainException>()
            .WithMessage(AppErrorMessages.ProfileDoesNotExistMessage);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFarm_Test()
    {
        var profileId = _data.ThatHasNoFarm.Id;
        var dto = new CreateFarmDto
        {
            Name = "Farm"
        };

        var timeService = new Mock<ITimeService>();
        timeService.Setup(x => x.Now()).Returns(Now);

        var sut = new WritableFarmProvider(_data.UnitOfWork, timeService.Object);

        var farm = await sut.CreateAsync(profileId, dto);

        farm.OwnerId.Should().Be(profileId);
        farm.FarmStatistics.Should().NotBeNull();
        farm.Name.Should().Be("Farm");
    }
}