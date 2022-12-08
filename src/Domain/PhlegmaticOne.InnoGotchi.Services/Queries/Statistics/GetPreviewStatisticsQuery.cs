using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.Statistics;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Statistics;

public class GetPreviewStatisticsQuery : IdentityOperationResultQuery<PreviewFarmStatisticsDto>
{
    public GetPreviewStatisticsQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetPreviewStatisticsQueryHandler :
    IOperationResultQueryHandler<GetPreviewStatisticsQuery, PreviewFarmStatisticsDto>
{
    private readonly IValidator<ProfileFarmModel> _existsFarmValidator;
    private readonly IUnitOfWork _unitOfWork;

    public GetPreviewStatisticsQueryHandler(IUnitOfWork unitOfWork,
        IValidator<ProfileFarmModel> existsFarmValidator)
    {
        _unitOfWork = unitOfWork;
        _existsFarmValidator = existsFarmValidator;
    }

    public async Task<OperationResult<PreviewFarmStatisticsDto>> Handle(GetPreviewStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _existsFarmValidator
            .ValidateAsync(new ProfileFarmModel(request.ProfileId), cancellationToken);

        if (validationResult.IsValid == false)
            return OperationResult.Failed<PreviewFarmStatisticsDto>(validationResult.ToString());

        var repository = _unitOfWork.GetRepository<FarmStatistics>();

        var result = await repository.GetFirstOrDefaultAsync(
            predicate: p => p.Farm.OwnerId == request.ProfileId,
            selector: s => new PreviewFarmStatisticsDto
            {
                FarmId = s.FarmId,
                FarmName = s.Farm.Name,
                ProfileLastName = s.Farm.Owner.LastName,
                ProfileEmail = s.Farm.Owner.User.Email,
                ProfileFirstName = s.Farm.Owner.FirstName,
                PetsCount = s.Farm.InnoGotchies.Count
            }, cancellationToken: cancellationToken);

        return OperationResult.Successful(result!);
    }
}