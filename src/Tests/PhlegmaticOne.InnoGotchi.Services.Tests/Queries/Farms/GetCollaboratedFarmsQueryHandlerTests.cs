using FluentAssertions;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.Farms;

public class GetCollaboratedFarmsQueryHandlerTests
{
    private readonly UnitOfWorkMock _data;
    private readonly GetCollaboratedFarmsQueryHandler _sut;
    public GetCollaboratedFarmsQueryHandlerTests()
    {
        _data = UnitOfWorkMock.Create();

        var avatarProcessor = new Mock<IAvatarProcessor>();
        avatarProcessor.Setup(x => x.ProcessAvatarAsync(It.IsAny<Avatar>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Avatar
            {
                AvatarData = new byte[1]
            });

        _sut = new GetCollaboratedFarmsQueryHandler(_data.UnitOfWork, avatarProcessor.Object);
    }

    [Fact]
    public async Task ShouldReturnCollaboratedFarms_Test()
    {
        var profileId = _data.CreatedCollaboration.UserProfileId;
        var query = new GetCollaboratedFarmsQuery(profileId);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result.Should().HaveCount(1);
    }
}