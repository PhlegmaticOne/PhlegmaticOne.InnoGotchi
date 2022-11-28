using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.ValueConverters;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class ConstructorMapperConfiguration : Profile
{
    public ConstructorMapperConfiguration()
    {
        CreateMap<CreateInnoGotchiComponentViewModel, InnoGotchiModelComponentDto>();
        CreateMap<CreateInnoGotchiViewModel, CreateInnoGotchiDto>();
        CreateMap<IList<InnoGotchiComponentDto>, ConstructorViewModel>()
            .ForMember(x => x.ComponentCategories,
                o => o.ConvertUsing(new ComponentCategoriesValueConverter(), y => y));
    }
}