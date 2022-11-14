using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class InnoGotchiComponentsController
{
    private readonly IDataRepository<InnoGotchiComponent> _innoGotchiComponentsDataService;
    private readonly IMapper _mapper;
    private readonly IServerAddressProvider _serverAddressProvider;

    public InnoGotchiComponentsController(IDataService dataService, IMapper mapper, IServerAddressProvider serverAddressProvider)
    {
        _innoGotchiComponentsDataService = dataService.GetDataRepository<InnoGotchiComponent>();
        _mapper = mapper;
        _serverAddressProvider = serverAddressProvider;
    }

    [HttpGet]
    public async Task<OperationResult<InnoGotchiComponentCollectionDto>> GetAll()
    {
        var serverAddress = _serverAddressProvider.ServerAddressUri;
        var components = await _innoGotchiComponentsDataService.GetAllAsync();
        var result = _mapper.Map<InnoGotchiComponentCollectionDto>(components);
        result.Components.ForEach(component =>
        {
            component.ImageUrl = serverAddress.Combine(component.ImageUrl).AbsoluteUri;
        });
        return OperationResult.FromSuccess(result);
    }
}