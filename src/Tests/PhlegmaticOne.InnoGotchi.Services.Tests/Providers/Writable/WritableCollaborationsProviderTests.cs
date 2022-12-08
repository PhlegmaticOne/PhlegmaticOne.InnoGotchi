using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Domain.Exceptions;
using PhlegmaticOne.InnoGotchi.Services.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Providers.Writable;

public class WritableCollaborationsProviderTests
{
    private readonly UnitOfWorkMock _data;
    public WritableCollaborationsProviderTests() => _data = UnitOfWorkMock.Create();

    [Fact]
    public async Task CreateCollaborationAsync_ShouldThrowExceptionBecauseFromProfileDoesNotHaveFarm_Test()
    {
        var fromProfileId = _data.ThatHasNoFarm.Id;
        var toProfileId = _data.ThatHasFarm.Id;
        var sut = new WritableCollaborationsProvider(_data.UnitOfWork);

        await sut.Invoking(x => x.CreateCollaborationAsync(fromProfileId, toProfileId))
            .Should()
            .ThrowAsync<DomainException>()
            .WithMessage(AppErrorMessages.FarmDoesNotExistMessage);
    }

    [Fact]
    public async Task CreateCollaborationAsync_ShouldThrowExceptionBecauseToProfileDoesNotExist_Test()
    {
        var fromProfileId = _data.ThatHasFarm.Id;
        var toProfileId = Guid.Empty;
        var sut = new WritableCollaborationsProvider(_data.UnitOfWork);

        await sut.Invoking(x => x.CreateCollaborationAsync(fromProfileId, toProfileId))
            .Should()
            .ThrowAsync<DomainException>()
            .WithMessage(AppErrorMessages.ProfileDoesNotExistMessage);
    }

    [Fact]
    public async Task CreateCollaborationAsync_ShouldCreateCollaboration_Test()
    {
        var fromProfileId = _data.ThatHasFarm.Id;
        var toProfileId = _data.ThatHasNoFarm.Id;
        var sut = new WritableCollaborationsProvider(_data.UnitOfWork);

        var collaboration = await sut.CreateCollaborationAsync(fromProfileId, toProfileId);

        collaboration.Should().NotBeNull();
    }
}