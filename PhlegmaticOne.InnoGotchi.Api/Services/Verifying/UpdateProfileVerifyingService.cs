using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Services.Verifying.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Verifying;

public class UpdateProfileVerifyingService : VerifyingServiceBase<UpdateProfileDto, UserProfile>
{
    public UpdateProfileVerifyingService(IValidator<UpdateProfileDto> fluentValidator, IDataService dataService) :
        base(fluentValidator, dataService) { }

    public override Task<UserProfile> MapAsync(UpdateProfileDto from)
    {
        return (DataService.GetDataRepository<UserProfile>()
            .GetByIdOrDefaultAsync(from.Id, include: i => i.Include(x => x.User).Include(x => x.Avatar!)))!;
    }
}