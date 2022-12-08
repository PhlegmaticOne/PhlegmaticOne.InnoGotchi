using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Farms;

public class GetCollaboratedFarmsQuery : IdentityOperationResultQuery<IList<PreviewFarmDto>>
{
    public GetCollaboratedFarmsQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetCollaboratedFarmsQueryHandler :
    IOperationResultQueryHandler<GetCollaboratedFarmsQuery, IList<PreviewFarmDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAvatarProcessor _avatarProcessor;

    public GetCollaboratedFarmsQueryHandler(IUnitOfWork unitOfWork, IAvatarProcessor avatarProcessor)
    {
        _unitOfWork = unitOfWork;
        _avatarProcessor = avatarProcessor;
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

        await SetEmptyAvatars(result, cancellationToken);

        return OperationResult.Successful(result);
    }

    private async Task SetEmptyAvatars(IList<PreviewFarmDto> previewFarmDtos, CancellationToken cancellationToken)
    {
        foreach (var previewFarmDto in previewFarmDtos)
        {
            previewFarmDto.OwnerAvatarData =
                await _avatarProcessor.ProcessAvatarDataAsync(previewFarmDto.OwnerAvatarData, cancellationToken);
        }
    }
}