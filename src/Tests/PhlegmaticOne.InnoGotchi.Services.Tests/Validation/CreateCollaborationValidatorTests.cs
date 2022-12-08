using PhlegmaticOne.InnoGotchi.Services.Commands.Collaborations;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Validation;

public class CreateCollaborationValidatorTests
{
    private readonly UnitOfWorkMock _data;
    public CreateCollaborationValidatorTests() => _data = UnitOfWorkMock.Create();

    [Fact]
    public Task ShouldBeNotValid_BecauseToProfileDoesNotExist_Test() =>
        NotValidTestWithProfiles(AppErrorMessages.ProfileDoesNotExistMessage,
            () => (_data.ThatHasFarm.Id, Guid.Empty));

    [Fact]
    public Task ShouldBeNotValid_BecauseFromProfileDoesNotHaveFarm_Test() =>
        NotValidTestWithProfiles(AppErrorMessages.HaveNoFarmForCollaborationMessage,
            () => (_data.ThatHasNoFarm.Id, _data.ThatHasFarm.Id));

    [Fact]
    public Task ShouldBeNotValid_BecauseSuchCollaborationExists_Test() =>
        NotValidTestWithProfiles(AppErrorMessages.SuchCollaborationExistsMessage,
             () => (_data.CreatedCollaboration.Farm.OwnerId, _data.CreatedCollaboration.UserProfileId));

    [Fact]
    public async Task ShouldBeValid_Test()
    {
        var fromProfileId = _data.Profiles[2].Id;
        var toProfileId = _data.ThatHasNoFarm.Id;
        var collaborationCommand = new CreateCollaborationCommand(fromProfileId, toProfileId);
        var sut = new CreateCollaborationValidator(_data.UnitOfWork);

        var validationResult = await sut.ValidateAsync(collaborationCommand);

        ValidationResultAssertionHelper.AssertIsValid(validationResult);
    }

    private async Task NotValidTestWithProfiles(string errorMessage,
        Func<(Guid, Guid)> profilesGetter)
    {
        var (fromProfileId, toProfileId) = profilesGetter();
        var collaborationCommand = new CreateCollaborationCommand(fromProfileId, toProfileId);
        var sut = new CreateCollaborationValidator(_data.UnitOfWork);

        var validationResult = await sut.ValidateAsync(collaborationCommand);

        ValidationResultAssertionHelper.AssertNotValidWithSingleError(validationResult, errorMessage);
    }
}