using FluentAssertions;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Queries.Avatars;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.Avatars;

public class GetProfileAvatarQueryTests
{
    private readonly UnitOfWorkMock _data;
    private readonly GetProfileAvatarQueryHandler _sut;
    public GetProfileAvatarQueryTests()
    {
        _data = UnitOfWorkMock.Create();

        var avatarProcessor = new Mock<IAvatarProcessor>();
        avatarProcessor.Setup(x => x.ProcessAvatarAsync(It.IsAny<Avatar>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Avatar
            {
                AvatarData = new byte[1]
            });

        _sut = new GetProfileAvatarQueryHandler(_data.UnitOfWork, avatarProcessor.Object);
    }

    [Fact]
    public async Task ShouldReturnFailedResultBecauseProfileDoesNotExist_Test()
    {
        var request = new GetProfileAvatarQuery(Guid.Empty);

        var result = await _sut.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be(AppErrorMessages.ProfileDoesNotExistMessage);
    }

    [Fact]
    public async Task ShouldReturnAvatar_Test()
    {
        var profileId = _data.ThatHasFarm.Id;
        var request = new GetProfileAvatarQuery(profileId);

        var result = await _sut.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result!.AvatarData.Should().HaveCount(1);
    }
}