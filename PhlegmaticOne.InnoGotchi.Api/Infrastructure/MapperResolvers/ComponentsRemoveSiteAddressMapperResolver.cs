using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;

public class ComponentsRemoveSiteAddressMapperResolver : IValueResolver<CreateInnoGotchiDto, IdentityInnoGotchiModel, List<CreateInnoGotchiComponentDto>>
{
    private readonly IServerAddressProvider _serverAddressProvider;
    public ComponentsRemoveSiteAddressMapperResolver(IServerAddressProvider serverAddressProvider) => 
        _serverAddressProvider = serverAddressProvider;

    public List<CreateInnoGotchiComponentDto> Resolve(CreateInnoGotchiDto source, IdentityInnoGotchiModel destination, List<CreateInnoGotchiComponentDto> destMember,
        ResolutionContext context)
    {
        var serverAddress = _serverAddressProvider.ServerAddress;
        var serverAddressLength = serverAddress.Length;
        source.Components.ForEach(component =>
        {
            var imageUrl = component.ImageUrl;
            var imageUrlLength = imageUrl.Length;
            component.ImageUrl = imageUrl.Substring(serverAddressLength + 1, imageUrlLength - serverAddressLength - 1);
        });
        return source.Components;
    }
}