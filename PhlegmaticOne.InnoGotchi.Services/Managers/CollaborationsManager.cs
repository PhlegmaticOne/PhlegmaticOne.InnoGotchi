using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class CollaborationsManager : ICollaborationManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableCollaborationsProvider _writableCollaborationsProvider;
    private readonly IValidator<IdentityModel<CreateCollaborationDto>> _createValidator;

    public CollaborationsManager(IUnitOfWork unitOfWork,
        IWritableCollaborationsProvider writableCollaborationsProvider,
        IValidator<IdentityModel<CreateCollaborationDto>> createValidator)
    {
        _unitOfWork = unitOfWork;
        _writableCollaborationsProvider = writableCollaborationsProvider;
        _createValidator = createValidator;
    }

    public async Task<OperationResult> AddCollaboratorAsync(IdentityModel<CreateCollaborationDto> createCollaborationDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createCollaborationDto);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail(validationResult.ToString());
        }

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableCollaborationsProvider
                .AddCollaboration(createCollaborationDto.ProfileId, createCollaborationDto.Entity.Id);
        });
    }
}