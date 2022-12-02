using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Farms;

public class CreateFarmCommand : IdentityOperationResultCommandBase
{
    public CreateFarmCommand(Guid profileId, CreateFarmDto createFarmDto) : base(profileId)
    {
        CreateFarmDto = createFarmDto;
    }

    public CreateFarmDto CreateFarmDto { get; }
}

public class CreateFarmCommandHandler : IOperationResultCommandHandler<CreateFarmCommand>
{
    private readonly IValidator<CreateFarmCommand> _createFarmValidator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableFarmProvider _writableFarmProvider;

    public CreateFarmCommandHandler(IUnitOfWork unitOfWork,
        IValidator<CreateFarmCommand> createFarmValidator,
        IWritableFarmProvider writableFarmProvider)
    {
        _unitOfWork = unitOfWork;
        _createFarmValidator = createFarmValidator;
        _writableFarmProvider = writableFarmProvider;
    }

    public async Task<OperationResult> Handle(CreateFarmCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _createFarmValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false) return OperationResult.FromFail(validationResult.ToString());

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableFarmProvider
                .CreateAsync(request.ProfileId, request.CreateFarmDto, cancellationToken);
        });
    }
}