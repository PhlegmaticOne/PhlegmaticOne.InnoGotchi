using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.DataService.Models;
using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers.Base;

public class DataController : ControllerBase
{
    protected readonly IDataService DataService;
    protected readonly IMapper Mapper;

    public DataController(IDataService dataService, IMapper mapper)
    {
        DataService = dataService;
        Mapper = mapper;
    }

    protected Guid UserId() => User.GetUserId();
    protected async Task<OperationResult<TResult>> MapFromInsertionResult<TResult, TEntity>(TEntity entity) 
        where TEntity : EntityBase
    {
        var repository = DataService.GetDataRepository<TEntity>();
        var created = await repository.CreateAsync(entity);
        await DataService.SaveChangesAsync();
        return ResultFromMap<TResult>(created);
    }

    protected OperationResult<TResult> ResultFromMap<TResult>(object toMap)
    {
        var result = Mapper.Map<TResult>(toMap);
        return OperationResult.FromSuccess(result);
    }
}