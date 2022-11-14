using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Models.Base;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;

public static class MapperExtensions
{
    public static T MapIdentity<T>(this IMapper mapper, object obj, Guid profileId) where T : IdentityModelBase
    {
        var result = mapper.Map<T>(obj);
        result.ProfileId = profileId;
        return result;
    }
}