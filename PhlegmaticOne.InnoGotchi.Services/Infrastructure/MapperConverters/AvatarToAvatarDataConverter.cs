using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConverters;

public class AvatarToAvatarDataConverter : IValueConverter<Avatar?, byte[]>
{
    public byte[] Convert(Avatar? sourceMember, ResolutionContext context)
    {
        if (sourceMember is null)
        {
            return Array.Empty<byte>();
        }

        if (sourceMember.AvatarData is null)
        {
            return Array.Empty<byte>();
        }

        return sourceMember.AvatarData;
    }
}