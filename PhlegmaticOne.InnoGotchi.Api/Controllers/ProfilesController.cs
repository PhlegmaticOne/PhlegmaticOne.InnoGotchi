using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PasswordHasher.Base;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[AllowAnonymous]
public class ProfilesController : DataController
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IVerifyingService<RegisterProfileDto, UserProfile> _userProfileVerifyingService;

    public ProfilesController(IDataService dataService, IMapper mapper,
        IPasswordHasher passwordHasher,
        IVerifyingService<RegisterProfileDto, UserProfile> userProfileVerifyingService) : 
        base(dataService, mapper)
    {
        _passwordHasher = passwordHasher;
        _userProfileVerifyingService = userProfileVerifyingService;
    }
    
    [HttpPost]
    public async Task<OperationResult<AuthorizedProfileDto>> Register([FromBody] RegisterProfileDto registerProfileDto)
    {
        var validationResult = await _userProfileVerifyingService.ValidateAsync(registerProfileDto);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<AuthorizedProfileDto>(validationResult.ToString());
        }

        var userProfile = await _userProfileVerifyingService.MapAsync(registerProfileDto);
        return await MapFromInsertionResult<AuthorizedProfileDto, UserProfile>(userProfile);
    }

    [HttpPost]
    public async Task<OperationResult<AuthorizedProfileDto>> Login([FromBody] LoginDto loginDto)
    {
        var profile = await GetProfile(loginDto.Email);

        if (profile is null)
        {
            var notExistingUserErrorMessage = $"There is no user with email: {loginDto.Email}";
            return OperationResult.FromFail<AuthorizedProfileDto>(customMessage: notExistingUserErrorMessage);
        }

        if (PasswordsAreEqual(loginDto.Password, profile.User.Password) == false)
        {
            const string incorrectPasswordMessage = "You've entered incorrect password";
            return OperationResult.FromFail<AuthorizedProfileDto>(incorrectPasswordMessage);
        }

        return ResultFromMap<AuthorizedProfileDto>(profile);
    }

    [HttpGet]
    public async Task<OperationResult<DetailedProfileDto>> GetDetailed()
    {
        var repository = DataService.GetDataRepository<UserProfile>();
        var userProfile = await repository.GetByIdOrDefaultAsync(UserId(),
            i => i.Include(x => x.User).Include(x => x.Avatar!));
        return ResultFromMap<DetailedProfileDto>(userProfile!);
    }

    private Task<UserProfile?> GetProfile(string email) =>
        DataService.GetDataRepository<UserProfile>()
            .GetFirstOrDefaultAsync(x => x.User.Email == email, i => i.Include(x => x.User));

    private bool PasswordsAreEqual(string firstPassword, string secondPassword) =>
        _passwordHasher.Verify(firstPassword, secondPassword);
}