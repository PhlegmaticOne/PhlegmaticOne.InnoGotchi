using AutoMapper;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;

namespace PhlegmaticOne.InnoGotchi.Services.Tests;

public class MapperConfigurationTests
{
    [Fact]
    public void ShouldBeSuccess()
    {
        var mapping = new MapperConfiguration(_ => _.AddProfiles(
            typeof(CollaborationsMapperConfiguration).Assembly.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Profile)))
                .Select(type => (Profile)Activator.CreateInstance(type)!)));

        mapping.AssertConfigurationIsValid();
    }
}