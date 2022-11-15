using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.Other;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;

public class ComponentsRemoveSiteAddressMapperResolver : IValueResolver<CreateInnoGotchiDto, IdentityInnoGotchiModel, List<InnoGotchiModelComponentDto>>
{
    private readonly IServerAddressProvider _serverAddressProvider;
    public ComponentsRemoveSiteAddressMapperResolver(IServerAddressProvider serverAddressProvider) => 
        _serverAddressProvider = serverAddressProvider;

    public List<InnoGotchiModelComponentDto> Resolve(CreateInnoGotchiDto source, IdentityInnoGotchiModel destination, List<InnoGotchiModelComponentDto> destMember,
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