using FluentValidation;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.Statistics;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Statistics;

public class GetPreviewStatisticsQuery : IdentityOperationResultQueryBase<PreviewFarmStatisticsDto>
{
    public GetPreviewStatisticsQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetPreviewStatisticsQueryHandler :
    IOperationResultQueryHandler<GetPreviewStatisticsQuery, PreviewFarmStatisticsDto>
{
    private readonly IValidator<ExistsProfileFarmModel> _existsFarmValidator;
    private readonly IUnitOfWork _unitOfWork;

    public GetPreviewStatisticsQueryHandler(IUnitOfWork unitOfWork,
        IValidator<ExistsProfileFarmModel> existsFarmValidator)
    {
        _unitOfWork = unitOfWork;
        _existsFarmValidator = existsFarmValidator;
    }

    public async Task<OperationResult<PreviewFarmStatisticsDto>> Handle(GetPreviewStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _existsFarmValidator
            .ValidateAsync(new ExistsProfileFarmModel(request.ProfileId), cancellationToken);

        if (validationResult.IsValid == false)
            return OperationResult.FromFail<PreviewFarmStatisticsDto>(validationResult.ToString());

        var repository = _unitOfWork.GetRepository<Domain.Models.FarmStatistics>();

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

        return OperationResult.FromSuccess(result!);
    }
}