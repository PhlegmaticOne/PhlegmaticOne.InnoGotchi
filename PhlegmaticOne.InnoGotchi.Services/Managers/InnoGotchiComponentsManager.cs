using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class InnoGotchiComponentsManager : IInnoGotchiComponentsManager
{
    private readonly IInnoGotchiComponentsProvider _innoGotchiComponentsProvider;
    private readonly IMapper _mapper;

    public InnoGotchiComponentsManager(IInnoGotchiComponentsProvider innoGotchiComponentsProvider, IMapper mapper)
    {
        _innoGotchiComponentsProvider = innoGotchiComponentsProvider;
        _mapper = mapper;
    }
    public async Task<OperationResult<InnoGotchiComponentCollectionDto>> GetAllComponentsAsync()
    {
        var result = await _innoGotchiComponentsProvider.GetAllAsync();
        var mapped = _mapper.Map<InnoGotchiComponentCollectionDto>(result.Result!);
        return OperationResult.FromSuccess(mapped);
    }
}