using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableInnoGotchiProvider : IReadableInnoGotchiProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadableInnoGotchiProvider(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InnoGotchiModel> GetDetailedAsync(Guid petId, CancellationToken cancellationToken = new())
    {
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var pet = await repository.GetByIdOrDefaultAsync(petId,
            include: IncludeComponents(),
            cancellationToken: cancellationToken);
        return pet!;
    }

    private static Func<IQueryable<InnoGotchiModel>, IIncludableQueryable<InnoGotchiModel, object>> IncludeComponents()
    {
        return i => i.Include(x => x.Components).ThenInclude(x => x.InnoGotchiComponent);
    }
}