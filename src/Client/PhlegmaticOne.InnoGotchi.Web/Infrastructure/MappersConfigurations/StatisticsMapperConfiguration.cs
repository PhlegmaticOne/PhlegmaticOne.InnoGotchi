using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Statistics;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Statistics;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class StatisticsMapperConfiguration : Profile
{
    public StatisticsMapperConfiguration()
    {
        CreateMap<PreviewFarmStatisticsDto, PreviewFarmStatisticsViewModel>();
        CreateMap<DetailedFarmStatisticsDto, DetailedFarmStatisticsViewModel>();
        CreateMap<GlobalStatisticsDto, GlobalStatisticsViewModel>();
    }
}