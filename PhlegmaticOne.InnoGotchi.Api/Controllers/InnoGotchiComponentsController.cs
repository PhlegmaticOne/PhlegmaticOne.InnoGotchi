using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class InnoGotchiComponentsController
{
    private readonly IDataRepository<InnoGotchiComponent> _innoGotchiComponentsDataService;
    private readonly IMapper _mapper;

    public InnoGotchiComponentsController(IDataService dataService, IMapper mapper)
    {
        _innoGotchiComponentsDataService = dataService.GetDataRepository<InnoGotchiComponent>();
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