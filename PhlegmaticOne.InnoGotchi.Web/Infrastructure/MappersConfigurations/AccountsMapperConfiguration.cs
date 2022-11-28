using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Accounts;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class AccountsMapperConfiguration : Profile
{
    public AccountsMapperConfiguration()
    {
        CreateMap<SearchProfileDto, SearchProfileViewModel>();
    }
}