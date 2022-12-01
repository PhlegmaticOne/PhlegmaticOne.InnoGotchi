using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.FarmStatistics;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class FarmStatisticsMapperConfiguration : Profile
{
    public FarmStatisticsMapperConfiguration()
    {
        CreateMap<PreviewFarmStatisticsDto, PreviewFarmStatisticsViewModel>();
        CreateMap<DetailedFarmStatisticsDto, DetailedFarmStatisticsViewModel>();
    }
}