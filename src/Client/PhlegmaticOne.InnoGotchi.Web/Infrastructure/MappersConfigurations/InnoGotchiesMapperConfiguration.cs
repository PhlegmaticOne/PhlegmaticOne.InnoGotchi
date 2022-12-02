using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.ValueConverters;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Components;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;
using PhlegmaticOne.PagedLists.Implementation;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class InnoGotchiesMapperConfiguration : Profile
{
    public InnoGotchiesMapperConfiguration()
    {
        CreateMap<InnoGotchiModelComponentDto, InnoGotchiComponentViewModel>();
        CreateMap<PreviewInnoGotchiDto, PreviewInnoGotchiViewModel>();
        CreateMap<DetailedInnoGotchiDto, DetailedInnoGotchiViewModel>()
            .ForMember(x => x.DeadSince, o => o.ConvertUsing(new TimeToLocalZoneConverter(), x => x.DeadSince))
            .ForMember(x => x.AgeUpdatedAt, o => o.ConvertUsing(new TimeToLocalZoneConverter(), x => x.AgeUpdatedAt))
            .ForMember(x => x.LastDrinkTime, o => o.ConvertUsing(new TimeToLocalZoneConverter(), x => x.LastDrinkTime))
            .ForMember(x => x.LastFeedTime, o => o.ConvertUsing(new TimeToLocalZoneConverter(), x => x.LastFeedTime))
            .ForMember(x => x.LiveSince, o => o.ConvertUsing(new TimeToLocalZoneConverter(), x => x.LiveSince));

        CreateMap<DetailedInnoGotchiDto, PreviewInnoGotchiViewModel>();
        CreateMap<ReadonlyInnoGotchiPreviewDto, ReadonlyInnoGotchiPreviewViewModel>();
        CreateMap<PagedList<ReadonlyInnoGotchiPreviewDto>, PagedList<ReadonlyInnoGotchiPreviewViewModel>>();
    }
}