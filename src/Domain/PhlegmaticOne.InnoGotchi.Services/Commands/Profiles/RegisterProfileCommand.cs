using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Extensions;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;

public class RegisterProfileCommand : IOperationResultCommand
{
    public RegisterProfileCommand(RegisterProfileDto registerProfileModel) => RegisterProfileModel = registerProfileModel;

    public RegisterProfileDto RegisterProfileModel { get; }
}

public class RegisterProfileCommandHandler : ValidatableCommandHandler<RegisterProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableProfilesProvider _writableProfilesProvider;

    public RegisterProfileCommandHandler(IUnitOfWork unitOfWork,
        IWritableProfilesProvider writableProfilesProvider,
        IValidator<RegisterProfileCommand> registerProfileValidator) : base(registerProfileValidator) => 
        (_unitOfWork, _writableProfilesProvider) = (unitOfWork, writableProfilesProvider);

    protected override Task<OperationResult> HandleValidCommand(RegisterProfileCommand request, CancellationToken cancellationToken) =>
        _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableProfilesProvider
                .CreateAsync(request.RegisterProfileModel, cancellationToken);
        });
}