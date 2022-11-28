using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
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

    public async Task<Farm> CreateAsync(IdentityModel<CreateFarmDto> createFarmDto)
    {
        var farm = await CreateFarm(createFarmDto);
        var repository = _unitOfWork.GetRepository<Farm>();
        return await repository.CreateAsync(farm);
    }

    private async Task<Farm> CreateFarm(IdentityModel<CreateFarmDto> createFarmDto)
    {
        var userProfilesRepository = _unitOfWork.GetRepository<UserProfile>();
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