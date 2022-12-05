using AutoFixture;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Services.Tests.Helpers;
using PhlegmaticOne.InnoGotchi.Services.Tests.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Validation;

public class UpdateInnoGotchiValidatorTests
{
    private readonly UnitOfWorkData _data;
    public UpdateInnoGotchiValidatorTests() => _data = UnitOfWorkMock.Create();
    [Fact]
    public async Task ShouldBeNotValid_BecausePetIsDead_Test()
    {
        var profileId = _data.ThatHasFarm.Id;
        var petId = _data.DeadPet.Id;
        var updateInnoGotchiCommand = new UpdateInnoGotchiCommand(profileId, new()
        {
            PetId = petId,
            InnoGotchiOperationType = InnoGotchiOperationType.Drinking
        });
        var ownChecker = new Mock<IInnoGotchiOwnChecker>();
        ownChecker.Setup(x => x.IsBelongAsync(profileId, petId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = new UpdateInnoGotchiValidator(ownChecker.Object, _data.UnitOfWork);

        var validationResult = await sut.ValidateAsync(updateInnoGotchiCommand);

        ValidationResultAssertionHelper.AssertNotValidWithSingleError(validationResult,
            AppErrorMessages.CannotUpdateDeadPetMessage);
    }

    [Fact]
    public async Task ShouldBeValid_Test()
    {
        var profileId = _data.AlivePet.Farm.OwnerId;
        var petId = _data.AlivePet.Id;
        var updateInnoGotchiCommand = new UpdateInnoGotchiCommand(profileId, new()
        {
            PetId = petId,
            InnoGotchiOperationType = InnoGotchiOperationType.Drinking
        });
        var ownChecker = new Mock<IInnoGotchiOwnChecker>();
        ownChecker.Setup(x => x.IsBelongAsync(profileId, petId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = new UpdateInnoGotchiValidator(ownChecker.Object, _data.UnitOfWork);

        var validationResult = await sut.ValidateAsync(updateInnoGotchiCommand);

        ValidationResultAssertionHelper.AssertIsValid(validationResult);
    }
}