using AutoMapper;
using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.HelpModels;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies.Base;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.OperationResults.Mediatr;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.InnoGotchies;

public class GetDetailedInnoGotchiQuery : IdentityOperationResultQueryBase<DetailedInnoGotchiDto>
{
    public GetDetailedInnoGotchiQuery(Guid profileId, Guid petId) : base(profileId)
    {
        PetId = petId;
    }

    public Guid PetId { get; }
}

public class GetDetailedInnoGotchiQueryHandler :
    IOperationResultQueryHandler<GetDetailedInnoGotchiQuery, DetailedInnoGotchiDto>
{
    private readonly IValidator<GetDetailedInnoGotchiQuery> _innoGotchiValidator;
    private readonly IMapper _mapper;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiProvider;

    public GetDetailedInnoGotchiQueryHandler(IReadableInnoGotchiProvider readableInnoGotchiProvider,
        IMapper mapper,
        IValidator<GetDetailedInnoGotchiQuery> innoGotchiValidator)
    {
        _readableInnoGotchiProvider = readableInnoGotchiProvider;
        _mapper = mapper;
        _innoGotchiValidator = innoGotchiValidator;
    }

    public async Task<OperationResult<DetailedInnoGotchiDto>> Handle(GetDetailedInnoGotchiQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _innoGotchiValidator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
            return OperationResult.FromFail<DetailedInnoGotchiDto>(validationResult.ToString());

        var pet = await _readableInnoGotchiProvider.GetDetailedAsync(request.PetId, cancellationToken);

        var mapped = _mapper.Map<DetailedInnoGotchiDto>(pet);
        return OperationResult.FromSuccess(mapped);
    }
}