using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.ValueConverters;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Account;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class AccountMapperConfiguration : Profile
{
    public AccountMapperConfiguration()
    {
        CreateMap<RegisterViewModel, RegisterProfileDto>()
            .ForMember(x => x.AvatarData, o => o.ConvertUsing(new FormFileToByteArrayConverter(), y => y.Avatar));
        CreateMap<LoginViewModel, LoginDto>();
        CreateMap<DetailedProfileDto, ProfileViewModel>();
    }
}