using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchies;

public class GetDetailedInnoGotchiQuery : IdentityOperationResultQuery<DetailedInnoGotchiDto>
{
    public GetDetailedInnoGotchiQuery(Guid profileId, Guid petId) : base(profileId)
    {
        PetId = petId;
    }

    public Guid PetId { get; }
}

public class GetDetailedInnoGotchiQueryHandler :
    ValidatableQueryHandler<GetDetailedInnoGotchiQuery, DetailedInnoGotchiDto>
{
    private readonly IMapper _mapper;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiProvider;

    public GetDetailedInnoGotchiQueryHandler(IReadableInnoGotchiProvider readableInnoGotchiProvider,
        IMapper mapper,
        IValidator<GetDetailedInnoGotchiQuery> innoGotchiValidator) : base(innoGotchiValidator)
    {
        _readableInnoGotchiProvider = readableInnoGotchiProvider;
        _mapper = mapper;
    }

    protected override async Task<OperationResult<DetailedInnoGotchiDto>> HandleValidQuery(
        GetDetailedInnoGotchiQuery request, CancellationToken cancellationToken)
    {
        var pet = await _readableInnoGotchiProvider.GetDetailedAsync(request.PetId, cancellationToken);
        var mapped = _mapper.Map<DetailedInnoGotchiDto>(pet);
        return OperationResult.Successful(mapped);
    }
}