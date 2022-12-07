using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Services.Queries.Farms;
using PhlegmaticOne.InnoGotchi.Services.Tests.Mocks;
using PhlegmaticOne.InnoGotchi.Services.Tests.TestsInfrastructre;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Validation;

public class GetFarmByIdValidatorTests
{
    private readonly UnitOfWorkData _data;
    public GetFarmByIdValidatorTests() => _data = UnitOfWorkMock.Create();

    [Fact]
    public async Task ShouldBeNotValid_BecauseProfileDoesNotHaveCollaborationToSeeFarm_Test()
    {
        var profileId = Guid.Empty;
        var farmId = _data.CreatedCollaboration.FarmId;
        var getFarmByIdQuery = new GetFarmByIdQuery(profileId, farmId);
        var sut = new GetFarmByIdValidator(_data.UnitOfWork);

        var validationResult = await sut.ValidateAsync(getFarmByIdQuery);

        ValidationResultAssertionHelper.AssertNotValidWithSingleError(validationResult,
            AppErrorMessages.CannotGetFarmBecauseOfCollaboration);
    }

    [Fact]
    public async Task ShouldBeValid_Test()
    {
        var profileId = _data.CreatedCollaboration.UserProfileId;
        var farmId = _data.CreatedCollaboration.FarmId;
        var getFarmByIdQuery = new GetFarmByIdQuery(profileId, farmId);
        var sut = new GetFarmByIdValidator(_data.UnitOfWork);

        var validationResult = await sut.ValidateAsync(getFarmByIdQuery);

        ValidationResultAssertionHelper.AssertIsValid(validationResult);
    }
}