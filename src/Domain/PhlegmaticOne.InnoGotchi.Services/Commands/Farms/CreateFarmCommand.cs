using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Extensions;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Farms;

public class CreateFarmCommand : IdentityOperationResultCommand
{
    public CreateFarmCommand(Guid profileId, CreateFarmDto createFarmDto) : base(profileId) => CreateFarmDto = createFarmDto;

    public CreateFarmDto CreateFarmDto { get; }
}

public class CreateFarmCommandHandler : ValidatableCommandHandler<CreateFarmCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableFarmProvider _writableFarmProvider;

    public CreateFarmCommandHandler(IUnitOfWork unitOfWork,
        IWritableFarmProvider writableFarmProvider,
        IValidator<CreateFarmCommand> createFarmValidator) : base(createFarmValidator) =>
        (_unitOfWork, _writableFarmProvider) = (unitOfWork, writableFarmProvider);

    protected override Task<OperationResult> HandleValidCommand(CreateFarmCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableFarmProvider
                .CreateAsync(request.ProfileId, request.CreateFarmDto, cancellationToken);
        });
}