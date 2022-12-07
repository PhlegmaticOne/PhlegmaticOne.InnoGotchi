using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.Statistics;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Statistics;

public class GetDetailedStatisticsQuery : IdentityOperationResultQuery<DetailedFarmStatisticsDto>
{
    public GetDetailedStatisticsQuery(Guid profileId) : base(profileId)
    {
    }
}

public class GetDetailedStatisticsQueryHandler :
    IOperationResultQueryHandler<GetDetailedStatisticsQuery, DetailedFarmStatisticsDto>
{
    private readonly IValidator<ProfileFarmModel> _existsFarmValidator;
    private readonly IUnitOfWork _unitOfWork;

    public GetDetailedStatisticsQueryHandler(IUnitOfWork unitOfWork,
        IValidator<ProfileFarmModel> existsFarmValidator)
    {
        _unitOfWork = unitOfWork;
        _existsFarmValidator = existsFarmValidator;
    }

    public async Task<OperationResult<DetailedFarmStatisticsDto>> Handle(GetDetailedStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _existsFarmValidator
            .ValidateAsync(new ProfileFarmModel(request.ProfileId), cancellationToken);

        if (validationResult.IsValid == false)
            return OperationResult.Failed<DetailedFarmStatisticsDto>(validationResult.ToString());

        return await Build(request.ProfileId, cancellationToken);
    }

    private async Task<OperationResult<DetailedFarmStatisticsDto>> Build(Guid profileId, CancellationToken cancellationToken)
    {
        var farmsRepository = _unitOfWork.GetRepository<Farm>();
        var farmStatistics = await farmsRepository.GetFirstOrDefaultAsync(
            predicate: p => p.OwnerId == profileId,
            selector: farm => new
            {
                PetsInfo = farm.InnoGotchies.GroupBy(x => x.IsDead)
                    .Select(x => new
                    {
                        IsDead = x.Key,
                        Count = x.Count(),
                        AverageAge = x.Select(d => d.Age).DefaultIfEmpty().Average()
                    }),

                AverageHappinessDaysCount = farm.InnoGotchies
                    .Select(x => x.HappinessDaysCount).DefaultIfEmpty().Average(),
                FarmStatistics = farm.FarmStatistics,
                Farm = farm
            }, cancellationToken: cancellationToken);

        return OperationResult.Successful(new DetailedFarmStatisticsDto
        {
            FarmName = farmStatistics!.Farm.Name,
            FarmId = farmStatistics.Farm.Id,
            PetsCount = farmStatistics.PetsInfo.Select(x => x.Count).DefaultIfEmpty().Sum(),
            AverageFeedingPeriod = farmStatistics.FarmStatistics.AverageFeedTime,
            AverageThirstQuenchingPeriod = farmStatistics.FarmStatistics.AverageDrinkTime,
            AverageHappinessDaysCount = farmStatistics.AverageHappinessDaysCount,
            AlivePetsCount = DynamicFirstValue<int>(farmStatistics.PetsInfo, false, s => s.Count),
            DeadPetsCount = DynamicFirstValue<int>(farmStatistics.PetsInfo, true, s => s.Count),
            AverageAlivePetsAge = DynamicFirstValue<double>(farmStatistics.PetsInfo, false, s => s.AverageAge),
            AverageDeadPetsAge = DynamicFirstValue<double>(farmStatistics.PetsInfo, true, s => s.AverageAge)
        });
    }

    //Lol
    private static T DynamicFirstValue<T>(IEnumerable<dynamic> petsInfo, 
        bool isDead, Func<dynamic, T> selector) =>
        petsInfo
            .Where(x => x.IsDead == isDead)
            .Select(selector)
            .DefaultIfEmpty()
            .First()!;
}