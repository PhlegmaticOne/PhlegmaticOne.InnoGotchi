using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.InnoGotchies;

public class CreateInnoGotchiCommand : IdentityOperationResultCommandBase
{
    public CreateInnoGotchiDto CreateInnoGotchiDto { get; }

    public CreateInnoGotchiCommand(Guid profileId, CreateInnoGotchiDto createInnoGotchiDto) : base(profileId) => 
        CreateInnoGotchiDto = createInnoGotchiDto;
}

public class CreateInnoGotchiCommandHandler : IOperationResultCommandHandler<CreateInnoGotchiCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateInnoGotchiCommand> _createValidator;
    private readonly IWritableInnoGotchiesProvider _writableInnoGotchiesProvider;

    public CreateInnoGotchiCommandHandler(IUnitOfWork unitOfWork,
        IValidator<CreateInnoGotchiCommand> createValidator,
        IWritableInnoGotchiesProvider writableInnoGotchiesProvider)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _writableInnoGotchiesProvider = writableInnoGotchiesProvider;
    }

    public async Task<OperationResult> Handle(CreateInnoGotchiCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail(validationResult.ToString());
        }

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableInnoGotchiesProvider
                .CreateAsync(request.ProfileId, request.CreateInnoGotchiDto, cancellationToken);
        });
    }
}