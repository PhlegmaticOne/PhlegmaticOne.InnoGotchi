using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Profiles.Base;

public abstract class
    GetAuthorizedProfileQueryHandlerBase<TRequest> : IOperationResultQueryHandler<TRequest, AuthorizedProfileDto>
    where TRequest : IOperationResultQuery<AuthorizedProfileDto>
{
    private readonly IJwtTokenGenerationService _jwtTokenGenerationService;

    protected readonly IUnitOfWork UnitOfWork;

    protected GetAuthorizedProfileQueryHandlerBase(IUnitOfWork unitOfWork,
        IJwtTokenGenerationService jwtTokenGenerationService)
    {
        UnitOfWork = unitOfWork;
        _jwtTokenGenerationService = jwtTokenGenerationService;
    }

    public async Task<OperationResult<AuthorizedProfileDto>> Handle(TRequest request,
        CancellationToken cancellationToken)
    {
        var authorizedProfile = await GetAuthorizedProfileAsync(request, cancellationToken);

        if (authorizedProfile is null)
        {
            return OperationResult.Failed<AuthorizedProfileDto>(AppErrorMessages.ProfileDoesNotExistMessage);
        }

        authorizedProfile.JwtToken = _jwtTokenGenerationService.GenerateJwtToken(authorizedProfile);

        return OperationResult.Successful(authorizedProfile);
    }

    protected abstract Task<AuthorizedProfileDto?> GetAuthorizedProfileAsync(TRequest request,
        CancellationToken cancellationToken);
}