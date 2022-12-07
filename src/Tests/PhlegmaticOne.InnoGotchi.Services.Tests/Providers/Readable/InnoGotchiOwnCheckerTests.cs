using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Services.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Tests.Mocks;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Providers.Readable;

public class InnoGotchiOwnCheckerTests
{
    private readonly UnitOfWorkData _data;
    public InnoGotchiOwnCheckerTests() => _data = UnitOfWorkMock.Create();

    [Fact]
    public async Task IsBelongAsync_ShouldReturnTrueBecausePetBelongsToProfile_Test()
    {
        var profileId = _data.AlivePet.Farm.OwnerId;
        var petId = _data.AlivePet.Id;
        var sut = new InnoGotchiOwnChecker(_data.UnitOfWork);

        var isBelong = await sut.IsBelongAsync(profileId, petId);

        isBelong.Should().BeTrue();
    }

    [Fact]
    public async Task IsBelongAsync_ShouldReturnTrueBecausePetIsFromCollaboratedFarm_Test()
    {
        var profileId = _data.CreatedCollaboration.UserProfileId;
        var petId = _data.AlivePet.Id;
        var sut = new InnoGotchiOwnChecker(_data.UnitOfWork);

        var isBelong = await sut.IsBelongAsync(profileId, petId);

        isBelong.Should().BeTrue();
    }

    [Fact]
    public async Task IsBelongAsync_ShouldReturnFalseBecausePetIsNotBelongToProfile_Test()
    {
        var profileId = Guid.Empty;
        var petId = _data.AlivePet.Id;
        var sut = new InnoGotchiOwnChecker(_data.UnitOfWork);

        var isBelong = await sut.IsBelongAsync(profileId, petId);

        isBelong.Should().BeFalse();
    }
}