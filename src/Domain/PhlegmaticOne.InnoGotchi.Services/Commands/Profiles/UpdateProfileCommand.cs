using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;

public class UpdateProfileCommand : IdentityOperationResultCommandBase
{
    public UpdateProfileDto UpdateProfileDto { get; }

    public UpdateProfileCommand(Guid profileId, UpdateProfileDto updateProfileDto) : base(profileId) => 
        UpdateProfileDto = updateProfileDto;
}

public class UpdateProfileCommandHandler : IOperationResultCommandHandler<UpdateProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableProfilesProvider _writableProfilesProvider;
    private readonly IValidator<UpdateProfileCommand> _updateProfileValidator;

    public UpdateProfileCommandHandler(IUnitOfWork unitOfWork,
        IWritableProfilesProvider writableProfilesProvider,
        IValidator<UpdateProfileCommand> updateProfileValidator)
    {
        _unitOfWork = unitOfWork;
        _writableProfilesProvider = writableProfilesProvider;
        _updateProfileValidator = updateProfileValidator;
    }

    public async Task<OperationResult> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateProfileValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<AuthorizedProfileDto>(validationResult.ToString());
        }

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableProfilesProvider
                .UpdateAsync(request.ProfileId, request.UpdateProfileDto, cancellationToken);
        });
    }
}