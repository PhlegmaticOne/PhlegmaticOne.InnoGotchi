using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Profiles;

public class RegisterProfileCommand : IOperationResultCommand
{
    public RegisterProfileCommand(RegisterProfileDto registerProfileModel)
    {
        RegisterProfileModel = registerProfileModel;
    }

    public RegisterProfileDto RegisterProfileModel { get; }
}

public class RegisterProfileCommandHandler : IOperationResultCommandHandler<RegisterProfileCommand>
{
    private readonly IValidator<RegisterProfileCommand> _registerProfileValidator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableProfilesProvider _writableProfilesProvider;

    public RegisterProfileCommandHandler(IUnitOfWork unitOfWork,
        IValidator<RegisterProfileCommand> registerProfileValidator,
        IWritableProfilesProvider writableProfilesProvider)
    {
        _unitOfWork = unitOfWork;
        _registerProfileValidator = registerProfileValidator;
        _writableProfilesProvider = writableProfilesProvider;
    }

    public async Task<OperationResult> Handle(RegisterProfileCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _registerProfileValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return OperationResult.FromFail<AuthorizedProfileDto>(validationResult.ToString());

        return await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _writableProfilesProvider
                .CreateAsync(request.RegisterProfileModel, cancellationToken);
        });
    }
}