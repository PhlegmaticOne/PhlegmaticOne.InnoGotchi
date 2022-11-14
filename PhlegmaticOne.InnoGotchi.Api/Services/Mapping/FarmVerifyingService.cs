using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Mapping;

public class FarmVerifyingService : VerifyingServiceBase<IdentityFarmModel, Farm>
{
    public FarmVerifyingService(IValidator<IdentityFarmModel> fluentValidator, IDataService dataService) :
        base(fluentValidator, dataService) { }

    public override async Task<Farm> MapAsync(IdentityFarmModel from)
    {
        var userProfilesRepository = DataService.GetDataRepository<UserProfile>();
        var userProfile = await userProfilesRepository.GetByIdOrDefaultAsync(from.ProfileId, 
            include:i => i.Include(x => x.User));

        return new()
        {
            Name = from.Name,
            Owner = userProfile!
        };
    }
}