using FluentAssertions;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;
using PhlegmaticOne.PasswordHasher;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Providers.Writable;

public class WritableProfileProviderTests
{
    private readonly UnitOfWorkMock _data;
    private readonly WritableProfileProvider _sut;
    private static readonly DateTime Now = DateTime.MinValue;
    public WritableProfileProviderTests()
    {
        _data = UnitOfWorkMock.Create();

        var timeService = new Mock<ITimeService>();
        timeService.Setup(x => x.Now()).Returns(Now);

        var passwordHasher = new Mock<IPasswordHasher>();
        passwordHasher.Setup(x => x.Hash(It.IsAny<string>()))
            .Returns((string x) => x);

        _sut = new WritableProfileProvider(_data.UnitOfWork, passwordHasher.Object, timeService.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateProfile_Test()
    {
        var createProfileDto = new RegisterProfileDto
        {
            Email = "setzest@gmail.com",
            FirstName = "ASDSAD",
            LastName = "ASDAStt",
            Password = "password"
        };

        var profile = await _sut.CreateAsync(createProfileDto);

        profile.FirstName.Should().Be(createProfileDto.FirstName);
        profile.LastName.Should().Be(createProfileDto.LastName);
        profile.User.Email.Should().Be(createProfileDto.Email);
        profile.User.Password.Should().Be(createProfileDto.Password);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProfile_Test()
    {
        var profile = _data.ThatHasFarm;
        var updateProfileDto = new UpdateProfileDto
        {
            LastName = "AAAAA",
            FirstName = "BBBBB",
            NewPassword = "NewPassword",
            OldPassword = profile.User.Password
        };

        var updated = await _sut.UpdateAsync(profile.Id, updateProfileDto);

        profile.FirstName.Should().Be(updateProfileDto.FirstName);
        profile.LastName.Should().Be(updateProfileDto.LastName);
        profile.User.Password.Should().Be(updateProfileDto.NewPassword);
    }
}