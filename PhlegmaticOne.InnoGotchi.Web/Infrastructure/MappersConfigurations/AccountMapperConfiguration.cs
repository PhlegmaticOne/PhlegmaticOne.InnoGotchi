using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class AccountMapperConfiguration : Profile
{
    public AccountMapperConfiguration()
    {
        CreateMap<RegisterViewModel, RegisterProfileDto>()
            .ForMember(x => x.AvatarData,
                o => o.ConvertUsing(new FormFileToByteArrayConverter(), y => y.Avatar));
        CreateMap<LoginViewModel, LoginDto>();
    }

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
}