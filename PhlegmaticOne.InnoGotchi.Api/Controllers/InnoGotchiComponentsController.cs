using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
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
    public async Task<OperationResult<InnoGotchiComponentCollectionDto>> GetAll()
    {
        var components = await _innoGotchiComponentsDataService.GetAllAsync();
        var result = _mapper.Map<InnoGotchiComponentCollectionDto>(components);

        return OperationResult.FromSuccess(result);
    }
}