using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Collaborations;

public class CreateCollaborationCommand : IdentityOperationResultCommandBase
{
    public Guid ToProfileId { get; }

    public CreateCollaborationCommand(Guid profileId, Guid toProfileId) : base(profileId) => 
        ToProfileId = toProfileId;
}


public class CreateCollaborationCommandHandler : IOperationResultCommandHandler<CreateCollaborationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableCollaborationsProvider _writableCollaborationsProvider;
    private readonly IValidator<CreateCollaborationCommand> _createValidator;

    public CreateCollaborationCommandHandler(IUnitOfWork unitOfWork,
        IWritableCollaborationsProvider writableCollaborationsProvider,
        IValidator<CreateCollaborationCommand> createValidator)
    {
        _unitOfWork = unitOfWork;
        _writableCollaborationsProvider = writableCollaborationsProvider;
        _createValidator = createValidator;
    }

    public async Task<OperationResult> Handle(CreateCollaborationCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail(validationResult.ToString());
        }

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableCollaborationsProvider
                .AddCollaboration(request.ProfileId, request.ToProfileId, cancellationToken);
        });
    }
}