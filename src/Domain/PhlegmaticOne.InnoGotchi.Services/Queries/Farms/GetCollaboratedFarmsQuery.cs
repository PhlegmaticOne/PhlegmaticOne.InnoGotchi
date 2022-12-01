using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms;

public class GetCollaboratedFarmsQuery : IdentityOperationResultQueryBase<IList<PreviewFarmDto>>
{
    public GetCollaboratedFarmsQuery(Guid profileId) : base(profileId) { }
}

public class GetCollaboratedFarmsQueryHandler :
    IOperationResultQueryHandler<GetCollaboratedFarmsQuery, IList<PreviewFarmDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDefaultAvatarService _defaultAvatarService;

    public GetCollaboratedFarmsQueryHandler(IUnitOfWork unitOfWork, IDefaultAvatarService defaultAvatarService)
    {
        _unitOfWork = unitOfWork;
        _defaultAvatarService = defaultAvatarService;
    }

    public async Task<OperationResult<IList<PreviewFarmDto>>> Handle(GetCollaboratedFarmsQuery request,
        CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Collaboration>();

        var result = await repository.GetAllAsync(
            predicate: p => p.UserProfileId == request.ProfileId,
            selector: s => new PreviewFarmDto
            {
                Email = s.Farm.Owner.User.Email,
                FirstName = s.Farm.Owner.FirstName,
                LastName = s.Farm.Owner.LastName,
                Name = s.Farm.Name,
                FarmId = s.FarmId,
                OwnerAvatarData = s.Farm.Owner.Avatar!.AvatarData
            }, cancellationToken: cancellationToken);

        await SetEmptyAvatars(result);

        return OperationResult.FromSuccess(result);
    }

    private async Task SetEmptyAvatars(IList<PreviewFarmDto> previewFarmDtos)
    {
        foreach (var previewFarmDto in previewFarmDtos)
        {
            if (previewFarmDto.OwnerAvatarData is null || previewFarmDto.OwnerAvatarData.Any() == false)
            {
                previewFarmDto.OwnerAvatarData = await _defaultAvatarService.GetDefaultAvatarDataAsync();
            }
        }
    }
}