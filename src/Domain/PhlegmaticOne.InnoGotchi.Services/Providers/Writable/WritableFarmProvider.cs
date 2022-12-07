using PhlegmaticOne.InnoGotchi.Domain.Exceptions;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableFarmProvider : IWritableFarmProvider
{
    private readonly ITimeService _timeService;
    private readonly IUnitOfWork _unitOfWork;

    public WritableFarmProvider(IUnitOfWork unitOfWork, ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _timeService = timeService;
    }

    public async Task<Farm> CreateAsync(Guid profileId, CreateFarmDto createFarmDto,
        CancellationToken cancellationToken = new())
    {
        var farm = await CreateFarm(profileId, createFarmDto, cancellationToken);
        var repository = _unitOfWork.GetRepository<Farm>();
        return await repository.CreateAsync(farm, cancellationToken);
    }

    private async Task<Farm> CreateFarm(Guid profileId, CreateFarmDto createFarmDto,
        CancellationToken cancellationToken = new())
    {
        var userProfile = await _unitOfWork
            .GetRepository<UserProfile>()
            .GetByIdOrDefaultAsync(profileId, cancellationToken: cancellationToken);

        if (userProfile is null)
        {
            throw new DomainException(AppErrorMessages.ProfileDoesNotExistMessage);
        }

        var now = _timeService.Now();
        return new Farm
        {
            Name = createFarmDto.Name,
            FarmStatistics = new FarmStatistics
            {
                LastDrinkTime = now,
                LastFeedTime = now
            },
            Owner = userProfile!
        };
    }
}