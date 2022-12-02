using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;
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

        CreateMap<DetailedProfileDto, ProfileViewModel>()
            .ForMember(x => x.JoinDate, o => o.ConvertUsing(new TimeToLocalZoneConverter(), x => x.JoinDate));

        CreateMap<DetailedProfileDto, UpdateAccountViewModel>()
            .ForMember(x => x.CurrentAvatar, o => o.MapFrom(x => Convert.ToBase64String(x.AvatarData)))
            .ForMember(x => x.OldFirstName, o => o.MapFrom(x => x.FirstName))
            .ForMember(x => x.OldLastName, o => o.MapFrom(x => x.LastName));

        CreateMap<UpdateAccountViewModel, UpdateProfileDto>()
            .ForMember(x => x.AvatarData, o => o.ConvertUsing(new FormFileToByteArrayConverter(), y => y.Avatar))
            .ForMember(x => x.FirstName, o => o.MapFrom(x => x.FirstName ?? ""))
            .ForMember(x => x.LastName, o => o.MapFrom(x => x.LastName ?? ""))
            .ForMember(x => x.NewPassword, o => o.MapFrom(x => x.NewPassword ?? ""))
            .ForMember(x => x.OldPassword, o => o.MapFrom(x => x.OldPassword ?? ""));
    }
}