using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Profiles;

public class GetDetailedProfileQuery : IdentityOperationResultQueryBase<DetailedProfileDto>
{
    public GetDetailedProfileQuery(Guid profileId) : base(profileId) { }
}

public class GetDetailedProfileQueryHandler :
    IOperationResultQueryHandler<GetDetailedProfileQuery, DetailedProfileDto>
{
    private readonly IDefaultAvatarService _defaultAvatarService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetDetailedProfileQueryHandler(
        IDefaultAvatarService defaultAvatarService,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _defaultAvatarService = defaultAvatarService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<DetailedProfileDto>> Handle(GetDetailedProfileQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.GetRepository<UserProfile>()
            .GetByIdOrDefaultAsync(request.ProfileId,
                include: i => i.Include(x => x.User).Include(x => x.Avatar)!,
                cancellationToken: cancellationToken);

        if (result is null)
        {
            return OperationResult.FromFail<DetailedProfileDto>("Profile doesn't exist");
        }

        if (result.Avatar is null || result.Avatar.AvatarData.Any() == false)
        {
            result.Avatar = new Avatar
            {
                AvatarData = await _defaultAvatarService.GetDefaultAvatarDataAsync(cancellationToken)
            };
        }

        var mapped = _mapper.Map<DetailedProfileDto>(result);
        return OperationResult.FromSuccess(mapped);
    }
}