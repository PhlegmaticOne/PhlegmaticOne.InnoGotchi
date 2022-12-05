using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Services.Tests.Helpers;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Mapper;

public class MapperConfigurationTests
{
    [Fact]
    public void MapperConfigurationsFromAssembly_ShouldBeValid_Test()
    {
        var mapperConfiguration = MapperConfigurationsScanner
            .ScanAssembly(typeof(ProfileMapperConfiguration).Assembly);
        mapperConfiguration.AssertConfigurationIsValid();
    }
}