using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.FarmStatistics;

public class GetDetailedStatisticsQuery : IdentityOperationResultQueryBase<DetailedFarmStatisticsDto>
{
    public GetDetailedStatisticsQuery(Guid profileId) : base(profileId) { }
}

public class GetDetailedStatisticsQueryHandler :
    IOperationResultQueryHandler<GetDetailedStatisticsQuery, DetailedFarmStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ExistsProfileFarmModel> _existsFarmValidator;

    public GetDetailedStatisticsQueryHandler(IUnitOfWork unitOfWork,
        IValidator<ExistsProfileFarmModel> existsFarmValidator)
    {
        _unitOfWork = unitOfWork;
        _existsFarmValidator = existsFarmValidator;
    }

    public async Task<OperationResult<DetailedFarmStatisticsDto>> Handle(GetDetailedStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _existsFarmValidator
            .ValidateAsync(new ExistsProfileFarmModel(request.ProfileId), cancellationToken);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<DetailedFarmStatisticsDto>(validationResult.ToString());
        }

        var repository = _unitOfWork.GetRepository<Domain.Models.FarmStatistics>();

        var farmStatistics = await repository.GetFirstOrDefaultAsync(
            predicate: p => p.Farm.OwnerId == request.ProfileId,
            include: i => i.Include(x => x.Farm).ThenInclude(x => x.InnoGotchies),
            cancellationToken: cancellationToken);

        var alivePets = GetAlivePets(farmStatistics!.Farm.InnoGotchies).ToList();
        var deadPets = GetDeadPets(farmStatistics.Farm.InnoGotchies).ToList();

        return OperationResult.FromSuccess(new DetailedFarmStatisticsDto
        {
            FarmName = farmStatistics.Farm.Name,
            FarmId = farmStatistics.FarmId,
            PetsCount = farmStatistics.Farm.InnoGotchies.Count,
            AverageFeedingPeriod = farmStatistics.AverageFeedTime,
            AverageThirstQuenchingPeriod = farmStatistics.AverageDrinkTime,
            AverageHappinessDaysCount = GetAverageHappinessDaysCount(farmStatistics.Farm.InnoGotchies),
            AlivePetsCount = alivePets.Count,
            DeadPetsCount = deadPets.Count,
            AverageAlivePetsAge = GetAverageAge(alivePets),
            AverageDeadPetsAge = GetAverageAge(deadPets)
        });
    }
    private static double GetAverageHappinessDaysCount(IList<InnoGotchiModel> innoGotchies) =>
        innoGotchies.Count == 0 ? 0 : innoGotchies.Average(x => x.HappinessDaysCount);
    private static double GetAverageAge(IList<InnoGotchiModel> innoGotchies) =>
        innoGotchies.Count == 0 ? 0 : innoGotchies.Average(x => x.Age);
    private static IEnumerable<InnoGotchiModel> GetAlivePets(IList<InnoGotchiModel> innoGotchies) =>
        innoGotchies.Where(x => x.DeadSince == DateTime.MinValue);
    private static IEnumerable<InnoGotchiModel> GetDeadPets(IList<InnoGotchiModel> innoGotchies) =>
        innoGotchies.Where(x => x.DeadSince != DateTime.MinValue);
}