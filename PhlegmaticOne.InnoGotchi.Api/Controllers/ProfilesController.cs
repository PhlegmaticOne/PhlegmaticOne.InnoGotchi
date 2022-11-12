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
    private readonly IVerifyingService<UpdateProfileDto, UserProfile> _updateProfileVerifyingService;

    public ProfilesController(IDataService dataService, IMapper mapper, IPasswordHasher passwordHasher,
        IVerifyingService<RegisterProfileDto, UserProfile> userProfileVerifyingService,
        IVerifyingService<UpdateProfileDto, UserProfile> updateProfileVerifyingService) : base(dataService, mapper)
    {
        _passwordHasher = passwordHasher;
        _userProfileVerifyingService = userProfileVerifyingService;
        _updateProfileVerifyingService = updateProfileVerifyingService;
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

    [HttpPost]
    public async Task<OperationResult<AuthorizedProfileDto>> Update([FromBody] UpdateProfileDto updateProfileDto)
    {
        var validationResult = await _updateProfileVerifyingService.ValidateAsync(updateProfileDto);

        if (validationResult.IsValid == false)
        {
            return OperationResult.FromFail<AuthorizedProfileDto>(validationResult.ToString());
        }

        var updated = await UpdateProfile(updateProfileDto);
        return ResultFromMap<AuthorizedProfileDto>(updated);
    }

    [HttpGet]
    public async Task<OperationResult<DetailedProfileDto>> GetDetailed()
    {
        var repository = DataService.GetDataRepository<UserProfile>();
        var userProfile = await repository.GetByIdOrDefaultAsync(UserId(), 
            i => i.Include(x => x.User).Include(x => x.Avatar!));
        return ResultFromMap<DetailedProfileDto>(userProfile!);
    }

    private async Task<UserProfile> UpdateProfile(UpdateProfileDto updateProfileDto)
    {
        var repository = DataService.GetDataRepository<UserProfile>();
        var userProfile = await _updateProfileVerifyingService.MapAsync(updateProfileDto);
        var updated = await repository.UpdateAsync(userProfile, profile =>
        {
            profile.FirstName = GetNewValueOrExisting(updateProfileDto.FirstName, profile.FirstName);
            profile.LastName = GetNewValueOrExisting(updateProfileDto.LastName, profile.LastName);
            profile.User.Password = ProcessPassword(profile.User.Password, updateProfileDto.NewPassword);
            profile.Avatar = ProcessAvatar(updateProfileDto.AvatarData, profile.Avatar);
        });
        await DataService.SaveChangesAsync();
        return updated;
    }

    private Task<UserProfile?> GetProfile(string email) =>
        DataService.GetDataRepository<UserProfile>()
            .GetFirstOrDefaultAsync(x => x.User.Email == email, i => i.Include(x => x.User));

    private bool PasswordsAreEqual(string firstPassword, string secondPassword) =>
        _passwordHasher.Verify(firstPassword, secondPassword);

    private string ProcessPassword(string oldPassword, string newPassword) => 
        string.IsNullOrEmpty(newPassword) ? oldPassword : _passwordHasher.Hash(newPassword);

    private static Avatar? ProcessAvatar(byte[] newAvatarData, Avatar? oldAvatar) => 
        newAvatarData.Any() == false ? oldAvatar : new() { AvatarData = newAvatarData };

    private static string GetNewValueOrExisting(string newValue, string existing) => 
        string.IsNullOrEmpty(newValue) ? newValue : existing;
}