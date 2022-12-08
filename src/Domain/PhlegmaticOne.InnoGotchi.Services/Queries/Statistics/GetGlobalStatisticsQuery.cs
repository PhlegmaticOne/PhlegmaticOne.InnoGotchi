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
        var result = await BuildStatistics(cancellationToken);
        return OperationResult.Successful(result);
    }

    private async Task<GlobalStatisticsDto> BuildStatistics(CancellationToken cancellationToken)
    {
        var innoGotchiesInfo = await _unitOfWork.GetRepository<InnoGotchiModel>()
            .GetAllAsync(selector: s => new
            {
                s.IsDead,
                s.Age,
                s.HappinessDaysCount
            }, cancellationToken: cancellationToken);
        var farmsCount = await _unitOfWork.GetRepository<Farm>()
            .CountAsync(cancellationToken: cancellationToken);
        var collaborationsCount = await _unitOfWork.GetRepository<Collaboration>()
            .CountAsync(cancellationToken: cancellationToken);
        var profilesCount = await _unitOfWork.GetRepository<UserProfile>()
            .CountAsync(cancellationToken: cancellationToken);

        var result = new GlobalStatisticsDto
        {
            DeadPetsCount = innoGotchiesInfo.Count == 0 ? 0 : innoGotchiesInfo.Count(x => x.IsDead),
            AlivePetsCount = innoGotchiesInfo.Count == 0 ? 0 : innoGotchiesInfo.Count(x => x.IsDead == false),
            AverageDaysHappinessCount = innoGotchiesInfo.Count == 0 ? 0 : innoGotchiesInfo.Average(x => x.HappinessDaysCount),
            PetMaxAge = innoGotchiesInfo.Count == 0 ? 0 : innoGotchiesInfo.Max(x => x.Age),
            PetMaxHappinessDaysCount = innoGotchiesInfo.Count == 0 ? 0 : innoGotchiesInfo.Max(x => x.HappinessDaysCount),
            DeadPetsAverageAge = innoGotchiesInfo.Where(x => x.IsDead).Select(x => x.Age).DefaultIfEmpty().Average(),
            AlivePetsAverageAge = innoGotchiesInfo.Where(x => x.IsDead == false).Select(x => x.Age).DefaultIfEmpty().Average(),
            CollaborationsCount = collaborationsCount,
            ProfilesCount = profilesCount,
            FarmsCount = farmsCount
        };
        result.PetsTotalCount = result.AlivePetsCount + result.DeadPetsCount;

        return result;
    }
}