using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Avatars;

public class GetProfileAvatarQuery : IdentityOperationResultQueryBase<Avatar>
{
    public GetProfileAvatarQuery(Guid profileId) : base(profileId) { }
}

public class GetProfileAvatarQueryHandler : IOperationResultQueryHandler<GetProfileAvatarQuery, Avatar>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDefaultAvatarService _defaultAvatarService;

    public GetProfileAvatarQueryHandler(IUnitOfWork unitOfWork,
        IDefaultAvatarService defaultAvatarService)
    {
        _unitOfWork = unitOfWork;
        _defaultAvatarService = defaultAvatarService;
    }

    public async Task<OperationResult<Avatar>> Handle(GetProfileAvatarQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.GetRepository<Avatar>().GetFirstOrDefaultAsync(
                x => x.UserProfile.Id == request.ProfileId,
                cancellationToken: cancellationToken);

        if (result is null || result.AvatarData.Any() == false)
        {
            result = new Avatar
            {
                AvatarData = await _defaultAvatarService.GetDefaultAvatarDataAsync(cancellationToken)
            };
        }

        return OperationResult.FromSuccess(result);
    }
}