using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.ValueConverters;

public class ComponentCategoriesValueConverter :
    IValueConverter<IList<InnoGotchiComponentDto>, IList<ComponentCategoryViewModel>>
{
    public IList<ComponentCategoryViewModel> Convert(IList<InnoGotchiComponentDto> sourceMember,
        ResolutionContext context)
    {
        return sourceMember.GroupBy(x => x.Name)
            .Select(x => new ComponentCategoryViewModel
            {
                CategoryName = x.Key,
                Components = x.Select(y => new ComponentViewModel
                {
                    ImageUrl = y.ImageUrl,
                    Name = x.Key
                }).ToList()
            }).ToList();
    }
}