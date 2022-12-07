using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Extensions;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;

public class UpdateProfileCommand : IdentityOperationResultCommand
{
    public UpdateProfileCommand(Guid profileId, UpdateProfileDto updateProfileDto) : base(profileId)
    {
        UpdateProfileDto = updateProfileDto;
    }

    public UpdateProfileDto UpdateProfileDto { get; }
}

public class UpdateProfileCommandHandler : ValidatableCommandHandler<UpdateProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableProfilesProvider _writableProfilesProvider;

    public UpdateProfileCommandHandler(IUnitOfWork unitOfWork,
        IWritableProfilesProvider writableProfilesProvider,
        IValidator<UpdateProfileCommand> updateProfileValidator) : base(updateProfileValidator) =>
        (_unitOfWork, _writableProfilesProvider) = (unitOfWork, writableProfilesProvider);

    protected override Task<OperationResult> HandleValidCommand(UpdateProfileCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableProfilesProvider
                .UpdateAsync(request.ProfileId, request.UpdateProfileDto, cancellationToken);
        });
}