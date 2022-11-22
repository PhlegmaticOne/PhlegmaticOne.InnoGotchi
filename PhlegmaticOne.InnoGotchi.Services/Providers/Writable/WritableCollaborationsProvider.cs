﻿using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableCollaborationsProvider : IWritableCollaborationsProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public WritableCollaborationsProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OperationResult<Collaboration>> AddCollaboration(Guid fromProfileId, Guid toProfileId)
    {
        var farm = await GetFarm(fromProfileId);
        var collaboration = await CreateCollaboration(toProfileId, farm);
        farm.Collaborations.Add(collaboration);

        return OperationResult.FromSuccess(collaboration);
    }

    private async Task<Collaboration> CreateCollaboration(Guid profileId, Farm farm)
    {
        return new()
        {
            Collaborator = await GetProfile(profileId),
            Farm = farm
        };
    }

    private Task<Farm> GetFarm(Guid profileId) =>
        _unitOfWork.GetDataRepository<Farm>().GetFirstOrDefaultAsync(
            predicate: p => p.OwnerId == profileId,
            include: i => i.Include(x => x.Collaborations))!;
    private Task<UserProfile> GetProfile(Guid profileId) =>
        _unitOfWork.GetDataRepository<UserProfile>().GetByIdOrDefaultAsync(profileId)!;
}