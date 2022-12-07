using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Extensions;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Collaborations;

public class CreateCollaborationCommand : IdentityOperationResultCommand
{
    public CreateCollaborationCommand(Guid profileId, Guid toProfileId) : base(profileId) => ToProfileId = toProfileId;
    public Guid ToProfileId { get; }
}

public class CreateCollaborationCommandHandler : ValidatableCommandHandler<CreateCollaborationCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableCollaborationsProvider _writableCollaborationsProvider;

    public CreateCollaborationCommandHandler(IUnitOfWork unitOfWork,
        IWritableCollaborationsProvider writableCollaborationsProvider,
        IValidator<CreateCollaborationCommand> createValidator) : base(createValidator)
        => (_unitOfWork, _writableCollaborationsProvider) = (unitOfWork, writableCollaborationsProvider);

    protected override Task<OperationResult> HandleValidCommand(CreateCollaborationCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableCollaborationsProvider
                .CreateCollaborationAsync(request.ProfileId, request.ToProfileId, cancellationToken);
        });
}