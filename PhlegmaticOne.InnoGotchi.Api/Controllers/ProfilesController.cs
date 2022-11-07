using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.InnoGotchi.Shared.OperationResults;
using PhlegmaticOne.PasswordHasher.Base;
using PhlegmaticOne.InnoGotchi.Api.Services;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProfilesController : Controller
{
    private readonly IUserProfilesDataService _profilesDataService;
    private readonly IUsersDataService _usersDataService;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public ProfilesController(IUserProfilesDataService profilesDataService, 
        IUsersDataService usersDataService,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _profilesDataService = profilesDataService;
        _usersDataService = usersDataService;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<OperationResult<ProfileDto>> Register([FromBody] RegisterProfileDto createUserDto)
    {
        if (await _usersDataService.ExistsAsync(createUserDto.Email))
        {
            return OperationResult<ProfileDto>
                .FromFail($"Unable to create user profile. User with email exists: {createUserDto.Email}");
        }

        var newUserProfile = CreateUserProfile(createUserDto);

        var createdProfile = await _profilesDataService.CreateProfileAsync(newUserProfile);

        return ResultFromMap(createdProfile);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<OperationResult<ProfileDto>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _usersDataService.GetByEmailAsync(loginDto.Email);

        if (user is null)
        {
            return OperationResult<ProfileDto>.FromFail($"There is no user with email: {loginDto.Email}");
        }

        if (PasswordsEqual(loginDto.Password, user.Password) == false)
        {
            return OperationResult<ProfileDto>.FromFail("Incorrect password");
        }

        var profile = await _profilesDataService.GetProfileForUserAsync(user);

        return ResultFromMap(profile);
    }

    private OperationResult<ProfileDto> ResultFromMap(UserProfile userProfile)
    {
        var mapped = _mapper.Map<ProfileDto>(userProfile, o =>
        {
            o.AfterMap((_, dest) => dest.JwtToken = CreateJwtToken(userProfile.User.Email));
        });

        return OperationResult<ProfileDto>.FromSuccess(mapped);
    }

    private JwtTokenDto CreateJwtToken(string email)
    {
        var tokenValue = _jwtTokenGenerator.GenerateToken(email);
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

        return new()
        {
            User = user,
            FirstName = profileDto.FirstName,
            SecondName = profileDto.SecondName
        };
    }
}