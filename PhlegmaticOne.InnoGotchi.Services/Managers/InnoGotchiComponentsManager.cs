using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class InnoGotchiComponentsManager : IInnoGotchiComponentsManager
{
    private readonly IReadableInnoGotchiComponentsProvider _readableInnoGotchiComponentsProvider;
    private readonly IMapper _mapper;

    public InnoGotchiComponentsManager(IReadableInnoGotchiComponentsProvider readableInnoGotchiComponentsProvider, IMapper mapper)
    {
        _readableInnoGotchiComponentsProvider = readableInnoGotchiComponentsProvider;
        _mapper = mapper;
    }

    public async Task<OperationResult<InnoGotchiComponentCollectionDto>> GetAllComponentsAsync()
    {
        var all = await _readableInnoGotchiComponentsProvider.GetAllComponentsAsync();
        var mapped = _mapper.Map<InnoGotchiComponentCollectionDto>(all.Result);
        return OperationResult.FromSuccess(mapped);
    }
}