using System.Reflection;
using AutoMapper;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Helpers;

public static class MapperConfigurationsScanner
{
    public static MapperConfiguration ScanAssembly(Assembly assembly)
    {
        return new MapperConfiguration(_ => _.AddProfiles(
            assembly.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Profile)))
                .Select(type => (Profile)Activator.CreateInstance(type)!)));
    }
}