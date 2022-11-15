using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Other;

public interface IAvatarConvertingService
{
    byte[] ConvertAvatar(Avatar? avatar);
}