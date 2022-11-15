using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.Time;
using PhlegmaticOne.InnoGotchi.Api.Services.Verifying.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Data.Models.Enums;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class InnoGotchiesController : DataController
{
    private readonly IVerifyingService<IdentityInnoGotchiModel, InnoGotchiModel> _innoGotchiVerifyingService;
    private readonly ITimeProvider _timeProvider;

    public InnoGotchiesController(IDataService dataService, IMapper mapper,
        IVerifyingService<IdentityInnoGotchiModel, InnoGotchiModel> innoGotchiVerifyingService,
        ITimeProvider timeProvider) :
        base(dataService, mapper)
    {
        _innoGotchiVerifyingService = innoGotchiVerifyingService;
        _timeProvider = timeProvider;
    }

    [HttpPost]
    public async Task<OperationResult<InnoGotchiDto>> Create([FromBody] CreateInnoGotchiDto createInnoGotchiDto)
    {
        var profileInnoGotchiModel = Mapper.MapIdentity<IdentityInnoGotchiModel>(createInnoGotchiDto, ProfileId());
        var validationResult = await _innoGotchiVerifyingService.ValidateAsync(profileInnoGotchiModel);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<InnoGotchiDto>(validationResult.ToDictionary(),
                validationResult.OnlyErrors());
        }

        var created = await _innoGotchiVerifyingService.MapAsync(profileInnoGotchiModel);
        return await MapFromInsertionResult<InnoGotchiDto, InnoGotchiModel>(created);
    }

    [HttpGet]
    public async Task<OperationResult<InnoGotchiDto>> Get(Guid innoGotchiId)
    {
        var repository = DataService.GetDataRepository<InnoGotchiModel>();

        var innoGotchi = await repository.GetByIdOrDefaultAsync(innoGotchiId,
            include: IncludeComponents(),
            predicate: WhereProfileIdIs(ProfileId()));

        if (innoGotchi is null)
        {
            const string errorMessage = "You haven't such InnoGotchi";
            return OperationResult.FromFail<InnoGotchiDto>(errorMessage: errorMessage);
        }

        return ResultFromMap<InnoGotchiDto>(innoGotchi);
    }

    [HttpGet]
    public async Task<OperationResult<InnoGotchiCollectionDto>> GetAllCollaborated()
    {
        var petsRepository = DataService.GetDataRepository<InnoGotchiModel>();
        var invitationsRepository = DataService.GetDataRepository<Invitation>();

        var collaboratorIds = await invitationsRepository.GetAllAsync(
                predicate: WhereProfileIsCollaborator(ProfileId()),
                selector: CollaboratorIds());

        var pets = petsRepository.GetAllAsync(
            include: IncludeComponents(),
            predicate: p => collaboratorIds.Contains(p.Farm.OwnerId));

        return ResultFromMap<InnoGotchiCollectionDto>(pets);
    }

    private static Func<IQueryable<InnoGotchiModel>, IIncludableQueryable<InnoGotchiModel, object>> IncludeComponents() =>
        i => i.Include(x => x.Components).ThenInclude(x => x.InnoGotchiComponent);
    private static Expression<Func<InnoGotchiModel, bool>> WhereProfileIdIs(Guid profileId) =>
        x => x.Farm.OwnerId == profileId;
    private static Expression<Func<Invitation, bool>> WhereProfileIsCollaborator(Guid profileId) =>
        p => p.FromProfileId == profileId && p.InvitationStatus == InvitationStatus.Accepted;
    private static Expression<Func<Invitation, Guid>> CollaboratorIds() =>
        p => p.ToProfileId;
}