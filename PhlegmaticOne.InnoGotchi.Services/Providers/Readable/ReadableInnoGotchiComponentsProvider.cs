using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableInnoGotchiComponentsProvider : IReadableInnoGotchiComponentsProvider
{
    private readonly IUnitOfWork _unitOfWork;
    public ReadableInnoGotchiComponentsProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    public async Task<IList<InnoGotchiComponent>> GetAllComponentsAsync()
    {
        return await _unitOfWork.GetRepository<InnoGotchiComponent>().GetAllAsync();
    }
}