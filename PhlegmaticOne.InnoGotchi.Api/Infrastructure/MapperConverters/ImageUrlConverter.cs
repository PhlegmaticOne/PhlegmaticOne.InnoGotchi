using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Services.Other;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConverters;

public class ImageUrlConverter : IValueConverter<string, string>
{
    private readonly IServerAddressProvider _serverAddressProvider;

    public ImageUrlConverter(IServerAddressProvider serverAddressProvider) => 
        _serverAddressProvider = serverAddressProvider;

    public string Convert(string sourceMember, ResolutionContext context)
    {
        var address = _serverAddressProvider.ServerAddressUri;
        return address.Combine(sourceMember).AbsoluteUri;
    }
}