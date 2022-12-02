using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchies;

public class GetPreviewInnoGotchiQuery : IOperationResultQuery<PreviewInnoGotchiDto>
{
    public GetPreviewInnoGotchiQuery(Guid petId)
    {
        PetId = petId;
    }

    public Guid PetId { get; }
}

public class GetPreviewInnoGotchiQueryHandler :
    IOperationResultQueryHandler<GetPreviewInnoGotchiQuery, PreviewInnoGotchiDto>
{
    private readonly IMapper _mapper;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiProvider;

    public GetPreviewInnoGotchiQueryHandler(IReadableInnoGotchiProvider readableInnoGotchiProvider,
        IMapper mapper)
    {
        _readableInnoGotchiProvider = readableInnoGotchiProvider;
        _mapper = mapper;
    }

    public async Task<OperationResult<PreviewInnoGotchiDto>> Handle(GetPreviewInnoGotchiQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _readableInnoGotchiProvider.GetDetailedAsync(request.PetId, cancellationToken);
        var mapped = _mapper.Map<PreviewInnoGotchiDto>(result);
        return OperationResult.FromSuccess(mapped);
    }
}