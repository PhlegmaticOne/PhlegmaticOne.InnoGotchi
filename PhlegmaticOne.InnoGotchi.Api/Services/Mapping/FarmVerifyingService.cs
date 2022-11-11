using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Mapping;

public class FarmVerifyingService : VerifyingServiceBase<ProfileFarmModel, Farm>
{
    public FarmVerifyingService(IValidator<ProfileFarmModel> fluentValidator, IDataService dataService) :
        base(fluentValidator, dataService) { }

    public override async Task<Farm> MapAsync(ProfileFarmModel from)
    {
        var userProfilesRepository = DataService.GetDataRepository<UserProfile>();
        var userProfile = await userProfilesRepository.GetByIdOrDefaultAsync(from.ProfileId, 
            include:i => i.Include(x => x.User));

        return new()
        {
            Name = from.FarmName,
            Owner = userProfile!
        };
    }
}