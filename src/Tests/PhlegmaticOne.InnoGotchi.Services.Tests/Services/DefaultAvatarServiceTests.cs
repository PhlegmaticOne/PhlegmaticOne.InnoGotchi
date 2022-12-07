using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Services.Services;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Services;

public class DefaultAvatarServiceTests
{
    [Fact]
    public async Task GetDefaultAvatarDataAsync_ShouldReturnData_Test()
    {
        var sut = new DefaultAvatarService();

        var data = await sut.GetDefaultAvatarDataAsync();

        data.Should().NotBeEmpty();
    }
}