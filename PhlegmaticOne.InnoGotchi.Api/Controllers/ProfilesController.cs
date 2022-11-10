using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.JwtTokensGeneration;
using PhlegmaticOne.JwtTokensGeneration.Models;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PasswordHasher.Base;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[AllowAnonymous]
public class ProfilesController : Controller
{
    private readonly IDataRepository<UserProfile> _profilesDataService;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public ProfilesController(IDataService dataService,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _profilesDataService = dataService.GetDataRepository<UserProfile>();
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost]
    public async Task<OperationResult<ProfileDto>> Register([FromBody] RegisterProfileDto registerProfileDto)
    {
        var isUserExists = await _profilesDataService
            .ExistsAsync(x => x.User.Email == registerProfileDto.Email);

        if (isUserExists)
        {
            var customErrorMessage = $"Unable to create user profile. User with email exists: {registerProfileDto.Email}";
            return OperationResult.FromFail<ProfileDto>(customMessage: customErrorMessage);
        }

        var newUserProfile = CreateUserProfile(registerProfileDto);

        var createdProfile = await _profilesDataService.CreateAsync(newUserProfile);

        return ResultFromMap(createdProfile);
    }

    [HttpPost]
    public async Task<OperationResult<ProfileDto>> Login([FromBody] LoginDto loginDto)
    {
        var profile = await _profilesDataService
            .GetFirstOrDefaultAsync(x => x.User.Email == loginDto.Email,
                i => i.Include(x => x.User));

        if (profile is null)
        {
            var notExistingUserErrorMessage = $"There is no user with email: {loginDto.Email}";
            return OperationResult.FromFail<ProfileDto>(customMessage: notExistingUserErrorMessage);
        }

        if (PasswordsEqual(loginDto.Password, profile.User.Password) == false)
        {
            const string incorrectPasswordMessage = "You've entered incorrect password";
            return OperationResult.FromFail<ProfileDto>(incorrectPasswordMessage);
        }

        return ResultFromMap(profile!);
    }

    private OperationResult<ProfileDto> ResultFromMap(UserProfile userProfile)
    {
        var mapped = _mapper.Map<ProfileDto>(userProfile, o =>
        {
            o.AfterMap((_, dest) => dest.JwtToken = CreateJwtToken(userProfile));
        });

        return OperationResult.FromSuccess(mapped);
    }

    private JwtTokenDto CreateJwtToken(UserProfile userProfile)
    {
        var userInfo = new UserRegisteringModel(userProfile.Id, userProfile.FirstName, userProfile.SecondName,
            userProfile.User.Email);
        var tokenValue = _jwtTokenGenerator.GenerateToken(userInfo);
        return new JwtTokenDto(tokenValue);
    }

    private bool PasswordsEqual(string firstPassword, string secondPassword) =>
        _passwordHasher.Verify(firstPassword, secondPassword);

    private UserProfile CreateUserProfile(RegisterProfileDto profileDto)
    {
        var password = _passwordHasher.Hash(profileDto.Password);

        var user = new User
        {
            Email = profileDto.Email,
            Password = password
        };

        var avatar = new Avatar
        {
            AvatarData = profileDto.AvatarData
        };

        return new()
        {
            User = user,
            FirstName = profileDto.FirstName,
            SecondName = profileDto.SecondName,
            Avatar = avatar
        };
    }
}