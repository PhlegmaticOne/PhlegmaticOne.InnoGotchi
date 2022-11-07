using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;

namespace PhlegmaticOne.InnoGotchi.Web.MappersConfigurations;

public class AccountMapperConfiguration : Profile
{
    public AccountMapperConfiguration()
    {
        CreateMap<RegisterViewModel, RegisterProfileDto>();
        CreateMap<LoginViewModel, LoginDto>();
    }
}