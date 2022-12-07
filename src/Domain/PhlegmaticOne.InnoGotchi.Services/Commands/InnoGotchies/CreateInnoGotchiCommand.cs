using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Extensions;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;

public class CreateInnoGotchiCommand : IdentityOperationResultCommand
{
    public CreateInnoGotchiCommand(Guid profileId, CreateInnoGotchiDto createInnoGotchiDto) : base(profileId) => CreateInnoGotchiDto = createInnoGotchiDto;
    public CreateInnoGotchiDto CreateInnoGotchiDto { get; }
}

public class CreateInnoGotchiCommandHandler : ValidatableCommandHandler<CreateInnoGotchiCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableInnoGotchiesProvider _writableInnoGotchiesProvider;

    public CreateInnoGotchiCommandHandler(IUnitOfWork unitOfWork,
        IWritableInnoGotchiesProvider writableInnoGotchiesProvider,
        IValidator<CreateInnoGotchiCommand> createValidator) : base(createValidator) =>
        (_unitOfWork, _writableInnoGotchiesProvider) = (unitOfWork, writableInnoGotchiesProvider);

    protected override Task<OperationResult> HandleValidCommand(CreateInnoGotchiCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableInnoGotchiesProvider
                .CreateAsync(request.ProfileId, request.CreateInnoGotchiDto, cancellationToken);
        });
}