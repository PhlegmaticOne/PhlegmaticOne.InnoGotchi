using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Statistics;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Statistics;

public class GetGlobalStatisticsQuery : IOperationResultQuery<GlobalStatisticsDto> { }

public class GetGlobalStatisticsQueryHandler : 
    IOperationResultQueryHandler<GetGlobalStatisticsQuery, GlobalStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGlobalStatisticsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult<GlobalStatisticsDto>> Handle(GetGlobalStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var profilesRepository = _unitOfWork.GetRepository<UserProfile>();
        var profilesInfo = await profilesRepository
            .GetAllAsync(
                predicate: p => p.Farm != null,
                selector: s => new
            {
                CollaborationsCount = s.Collaborations.Count,
                PetsInfo = new
                {
                    AverageHappinessDaysCount = s.Farm!.InnoGotchies
                        .Select(x => x.HappinessDaysCount).DefaultIfEmpty().Average(),
                    MaxAge = s.Farm.InnoGotchies
                        .Select(x => x.Age).DefaultIfEmpty().Max(),
                    MaxHappinessDaysCount = s.Farm.InnoGotchies
                        .Select(x => x.HappinessDaysCount).DefaultIfEmpty().Max(),
                    OtherInfo = s.Farm.InnoGotchies.GroupBy(x => x.IsDead)
                        .Select(x => new
                        {
                            IsDead = x.Key,
                            Count = x.Count(),
                            AverageAge = x.Select(d => d.Age).DefaultIfEmpty().Average()
                        })
                }
            }, cancellationToken: cancellationToken);

        var profilesCount = await profilesRepository.CountAsync(cancellationToken: cancellationToken);

        if (profilesInfo.Any() == false)
        {
            return OperationResult.FromSuccess(new GlobalStatisticsDto
            {
                ProfilesCount = profilesCount
            });
        }

        var otherInfos = profilesInfo.SelectMany(x => x.PetsInfo.OtherInfo).ToList();
        var result = new GlobalStatisticsDto
        {
            ProfilesCount = profilesCount,

            DeadPetsCount = otherInfos
                .Where(x => x.IsDead).Select(x => x.Count).DefaultIfEmpty().Sum(),
            AlivePetsCount = otherInfos
                .Where(x => x.IsDead == false).Select(x => x.Count).DefaultIfEmpty().Sum(),
            DeadPetsAverageAge = otherInfos
                .Where(x => x.IsDead).Select(x => x.AverageAge).DefaultIfEmpty().Average(),
            AlivePetsAverageAge = otherInfos
                .Where(x => x.IsDead == false).Select(x => x.AverageAge).DefaultIfEmpty().Average(),

            CollaborationsCount = profilesInfo.Sum(x => x.CollaborationsCount),
            AverageDaysHappinessCount = profilesInfo.Average(x => x.PetsInfo.AverageHappinessDaysCount),
            PetMaxAge = profilesInfo.Max(x => x.PetsInfo.MaxAge),
            PetMaxHappinessDaysCount = profilesInfo.Max(x => x.PetsInfo.MaxHappinessDaysCount),
            FarmsCount = profilesInfo.Count
        };
        result.PetsTotalCount = result.AlivePetsCount + result.DeadPetsCount;

        return OperationResult.FromSuccess(result);
    }
}