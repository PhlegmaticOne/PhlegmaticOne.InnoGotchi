using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Avatars;

public class GetProfileAvatarQuery : IdentityOperationResultQuery<Avatar>
{
    public GetProfileAvatarQuery(Guid profileId) : base(profileId) { }
}

public class GetProfileAvatarQueryHandler : IOperationResultQueryHandler<GetProfileAvatarQuery, Avatar>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAvatarProcessor _avatarProcessor;

    public GetProfileAvatarQueryHandler(IUnitOfWork unitOfWork, IAvatarProcessor avatarProcessor)
    {
        _unitOfWork = unitOfWork;
        _avatarProcessor = avatarProcessor;
    }

    public async Task<OperationResult<Avatar>> Handle(GetProfileAvatarQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork
            .GetRepository<Avatar>()
            .GetFirstOrDefaultAsync(
                predicate: x => x.UserProfile.Id == request.ProfileId,
                cancellationToken: cancellationToken);

        if (result is null)
        {
            return OperationResult.Failed<Avatar>(AppErrorMessages.ProfileDoesNotExistMessage);
        }

        result = await _avatarProcessor.ProcessAvatarAsync(result, cancellationToken);

        return OperationResult.Successful(result);
    }
}