using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;
using PhlegmaticOne.InnoGotchi.Shared.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class InnoGotchiComponentsController
{
    private readonly IInnoGotchiComponentsDataService _innoGotchiComponentsDataService;
    private readonly IMapper _mapper;

    public InnoGotchiComponentsController(IInnoGotchiComponentsDataService innoGotchiComponentsDataService, IMapper mapper)
    {
        _innoGotchiComponentsDataService = innoGotchiComponentsDataService;
        _mapper = mapper;
    }

    [HttpGet]
    //[Authorize]
    public async Task<OperationResult<InnoGotchiComponentCollectionDto>> GetAll()
    {
        var components = await _innoGotchiComponentsDataService.GetAllAsync();
        var result = _mapper.Map<InnoGotchiComponentCollectionDto>(components);

        return OperationResult<InnoGotchiComponentCollectionDto>.FromSuccess(result);
    }
}