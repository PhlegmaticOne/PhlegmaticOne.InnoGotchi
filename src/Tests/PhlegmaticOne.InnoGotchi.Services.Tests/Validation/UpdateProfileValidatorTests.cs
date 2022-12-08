using AutoFixture;
using PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.PasswordHasher.Implementation;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Validation;

public class UpdateProfileValidatorTests
{
    private readonly UnitOfWorkMock _data;
    public UpdateProfileValidatorTests() => _data = UnitOfWorkMock.Create();

    [Fact]
    public async Task ShouldBeNotValid_BecauseOldPasswordNotIncorrect_Test()
    {
        var profileId = _data.ThatHasFarm.Id;
        var registerDto = new Fixture()
            .Build<UpdateProfileDto>()
            .With(x => x.OldPassword, "OldPassword_1234")
            .With(x => x.NewPassword, "NewPassword_1234")
            .Create();
        var updateCommand = new UpdateProfileCommand(profileId, registerDto);
        var sut = new UpdateProfileValidator(_data.UnitOfWork, new SecurePasswordHasher());

        var validationResult = await sut.ValidateAsync(updateCommand);

        ValidationResultAssertionHelper.AssertNotValidWithSingleError(validationResult,
            AppErrorMessages.OldPasswordIsIncorrectMessage);
    }

    [Fact]
    public async Task ShouldBeValid_Test()
    {
        var profileId = _data.ThatHasFarm.Id;
        var oldPassword = _data.ThatHasFarm.User.Password;
        var registerDto = new Fixture()
            .Build<UpdateProfileDto>()
            .With(x => x.OldPassword, oldPassword)
            .With(x => x.NewPassword, "NewPassword_1234")
            .Create();
        var updateCommand = new UpdateProfileCommand(profileId, registerDto);
        var sut = new UpdateProfileValidator(_data.UnitOfWork, new SecurePasswordHasher());

        var validationResult = await sut.ValidateAsync(updateCommand);

        ValidationResultAssertionHelper.AssertNotValidWithSingleError(validationResult,
            AppErrorMessages.OldPasswordIsIncorrectMessage);
    }
}