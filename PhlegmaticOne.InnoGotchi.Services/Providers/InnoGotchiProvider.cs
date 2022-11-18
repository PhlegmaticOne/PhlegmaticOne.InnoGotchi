using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace PhlegmaticOne.InnoGotchi.Services.Providers;

public class InnoGotchiProvider : IInnoGotchiesProvider
{
    private readonly IUnitOfWork _unitOfWork;
    public InnoGotchiProvider(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<OperationResult<InnoGotchiModel>> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto)
    {
        InnoGotchiModel createdInnoGotchi;

        //await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var created = await CreateInnoGotchi(createInnoGotchiDto);
            var repository = _unitOfWork.GetDataRepository<InnoGotchiModel>();
            createdInnoGotchi = await repository.CreateAsync(created);
            await _unitOfWork.SaveChangesAsync();
            //await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            //await transaction.RollbackAsync();
            return OperationResult.FromFail<InnoGotchiModel>(e.Message);
        }

        return OperationResult.FromSuccess(createdInnoGotchi);
    }

    public async Task<OperationResult<InnoGotchiModel>> GetDetailedAsync(IdentityModel<Guid> petIdModel)
    {
        var repository = _unitOfWork.GetDataRepository<InnoGotchiModel>();

        var pet = await repository.GetByIdOrDefaultAsync(petIdModel.Entity,
            include: IncludeComponents(),
            predicate: WhereProfileIdIs(petIdModel.ProfileId));

        if (pet is null)
        {
            return OperationResult.FromFail<InnoGotchiModel>($"You haven't InnoGotchi with id {petIdModel.Entity}");
        }

        return OperationResult.FromSuccess(pet);
    }
    private static Func<IQueryable<InnoGotchiModel>, IIncludableQueryable<InnoGotchiModel, object>> IncludeComponents() =>
        i => i.Include(x => x.Components).ThenInclude(x => x.InnoGotchiComponent);
    private static Expression<Func<InnoGotchiModel, bool>> WhereProfileIdIs(Guid profileId) =>
        x => x.Farm.OwnerId == profileId;

    private async Task<InnoGotchiModel> CreateInnoGotchi(IdentityModel<CreateInnoGotchiDto> from)
    {
        var entity = from.Entity;
        var componentsToCreate = entity.Components;
        var components = await GetExistingComponents(componentsToCreate);
        var farm = await GetProfileFarm(from.ProfileId);
        var innoGotchiComponents = CreateModelComponents(componentsToCreate, components);
        var now = DateTime.Now;

        return new InnoGotchiModel
        {
            HungerLevel = HungerLevel.Normal,
            LastDrinkTime = now,
            LastFeedTime = now,
            Name = entity.Name,
            ThirstyLevel = ThirstyLevel.Normal,
            Components = innoGotchiComponents,
            Farm = farm,
            Age = 0,
            AgeUpdatedAt = now,
            HappinessDaysCount = 0,
            LiveSince = now
        };
    }

    private Task<IList<InnoGotchiComponent>> GetExistingComponents(List<InnoGotchiModelComponentDto> componentsToCreate)
    {
        var urls = componentsToCreate.Select(x => x.ImageUrl).ToList();
        var componentsRepository = _unitOfWork.GetDataRepository<InnoGotchiComponent>();
        return componentsRepository.GetAllAsync(predicate: x => urls.Contains(x.ImageUrl));
    }

    private Task<Farm> GetProfileFarm(Guid profileId)
    {
        var farmRepository = _unitOfWork.GetDataRepository<Farm>();
        return farmRepository.GetFirstOrDefaultAsync(x => x.OwnerId == profileId)!;
    }

    private static List<InnoGotchiModelComponent> CreateModelComponents(List<InnoGotchiModelComponentDto> dtos, IEnumerable<InnoGotchiComponent> existingComponents) =>
        dtos.Join(existingComponents, on => on.ImageUrl, on => on.ImageUrl,
            (dto, component) => new InnoGotchiModelComponent
            {
                InnoGotchiComponent = component,
                TranslationX = dto.TranslationX,
                TranslationY = dto.TranslationY,
                ScaleX = dto.ScaleX,
                ScaleY = dto.ScaleY
            }).ToList();
}