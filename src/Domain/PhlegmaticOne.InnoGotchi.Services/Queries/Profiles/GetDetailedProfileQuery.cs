using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Profiles;

public class GetDetailedProfileQuery : IdentityOperationResultQuery<DetailedProfileDto>
{
    public GetDetailedProfileQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetDetailedProfileQueryHandler :
    IOperationResultQueryHandler<GetDetailedProfileQuery, DetailedProfileDto>
{
    private readonly IAvatarProcessor _avatarProcessor;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetDetailedProfileQueryHandler(
        IAvatarProcessor avatarProcessor,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _avatarProcessor = avatarProcessor;
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
            return OperationResult.Failed<DetailedProfileDto>(AppErrorMessages.ProfileDoesNotExistMessage);
        }

        result.Avatar = await _avatarProcessor.ProcessAvatarAsync(result.Avatar, cancellationToken);

        var mapped = _mapper.Map<DetailedProfileDto>(result);
        return OperationResult.Successful(mapped);
    }
}