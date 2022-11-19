using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableFarmProvider : IWritableFarmProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITimeService _timeService;

    public WritableFarmProvider(IUnitOfWork unitOfWork, ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _timeService = timeService;
    }

    public async Task<OperationResult<Farm>> CreateAsync(IdentityModel<CreateFarmDto> createFarmDto)
    {
        var farm = await CreateFarm(createFarmDto);
        var repository = _unitOfWork.GetDataRepository<Farm>();
        var createdFarm = await repository.CreateAsync(farm);
        return OperationResult.FromSuccess(createdFarm);
    }

    private async Task<Farm> CreateFarm(IdentityModel<CreateFarmDto> createFarmDto)
    {
        var userProfilesRepository = _unitOfWork.GetDataRepository<UserProfile>();
        var userProfile = await userProfilesRepository.GetByIdOrDefaultAsync(createFarmDto.ProfileId,
            include: i => i.Include(x => x.User));

        var now = _timeService.Now();
        return new Farm
        {
            Name = createFarmDto.Entity.Name,
            FarmStatistics = new FarmStatistics
            {
                LastDrinkTime = now,
                LastFeedTime = now
            },
            Owner = userProfile!
        };
    }
}