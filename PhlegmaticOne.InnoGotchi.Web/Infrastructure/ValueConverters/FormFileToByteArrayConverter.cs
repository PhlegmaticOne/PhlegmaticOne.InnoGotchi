using AutoMapper;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.ValueConverters;

public class FormFileToByteArrayConverter : IValueConverter<IFormFile?, byte[]>
{
    public byte[] Convert(IFormFile? sourceMember, ResolutionContext context)
    {
        if (sourceMember is null)
        {
            return Array.Empty<byte>();
        }

        using var binaryReader = new BinaryReader(sourceMember.OpenReadStream());
        var avatarData = binaryReader.ReadBytes((int)sourceMember.Length);
        return avatarData;
    }
}