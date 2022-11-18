using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers;

public class InnoGotchiComponentsProvider : IInnoGotchiComponentsProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public InnoGotchiComponentsProvider(IUnitOfWork unitOfWork) => 
        _unitOfWork = unitOfWork;

    public async Task<OperationResult<IList<InnoGotchiComponent>>> GetAllAsync()
    {
        var all = await _unitOfWork.GetDataRepository<InnoGotchiComponent>()
            .GetAllAsync();
        return OperationResult.FromSuccess(all);
    }
}