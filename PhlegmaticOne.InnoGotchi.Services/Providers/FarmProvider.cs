using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers;

public class FarmProvider : IFarmProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public FarmProvider(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<OperationResult<Farm>> CreateAsync(IdentityModel<CreateFarmDto> createFarmDto)
    {
        var farm = await CreateFarm(createFarmDto);

       // await using var transaction = await _unitOfWork.BeginTransactionAsync();
        Farm createdFarm;

        try
        {
            var repository = _unitOfWork.GetDataRepository<Farm>();
            createdFarm = await repository.CreateAsync(farm);
            await _unitOfWork.SaveChangesAsync();
            //await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            //await transaction.RollbackAsync();
            return OperationResult.FromFail<Farm>(e.Message);
        }

        return OperationResult.FromSuccess(createdFarm);
    }

    public async Task<OperationResult<Farm>> GetWithPetsAsync(Guid profileId)
    {
        var farmDataService = _unitOfWork.GetDataRepository<Farm>();
        var farm = await farmDataService
            .GetFirstOrDefaultAsync(x => x.Owner.Id == profileId, include: IncludePets());

        if (farm is null)
        {
            return OperationResult.FromFail<Farm>("You don't have a farm");
        }

        return OperationResult.FromSuccess(farm);
    }

    private async Task<Farm> CreateFarm(IdentityModel<CreateFarmDto> createFarmDto)
    {
        var userProfilesRepository = _unitOfWork.GetDataRepository<UserProfile>();
        var userProfile = await userProfilesRepository.GetByIdOrDefaultAsync(createFarmDto.ProfileId,
            include: i => i.Include(x => x.User));

        return new Farm
        {
            Name = createFarmDto.Entity.Name,
            FarmStatistics = new FarmStatistics(),
            Owner = userProfile!
        };
    }

    private static Func<IQueryable<Farm>, IIncludableQueryable<Farm, object>> IncludePets() =>
        i => i.Include(x => x.InnoGotchies).ThenInclude(x => x.Components).ThenInclude(x => x.InnoGotchiComponent);
}