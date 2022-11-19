using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PasswordHasher.Base;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableProfileProvider : IWritableProfilesProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITimeService _timeService;

    public WritableProfileProvider(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _timeService = timeService;
    }
    public async Task<OperationResult<UserProfile>> CreateAsync(RegisterProfileDto registerProfileDto)
    {
        var prepared = PrepareProfile(registerProfileDto);
        var repository = _unitOfWork.GetDataRepository<UserProfile>();
        var createdProfile = await repository.CreateAsync(prepared);
        return OperationResult.FromSuccess(createdProfile);
    }


    public async Task<OperationResult<UserProfile>> UpdateAsync(UpdateProfileDto updateProfileDto)
    {
        var repository = _unitOfWork.GetDataRepository<UserProfile>();
        var updatedProfile = (await repository.UpdateAsync(updateProfileDto.Id, profile =>
        {
            profile.FirstName = GetNewValueOrExisting(updateProfileDto.FirstName, profile.FirstName);
            profile.LastName = GetNewValueOrExisting(updateProfileDto.LastName, profile.LastName);
            profile.User.Password = ProcessPassword(profile.User.Password, updateProfileDto.NewPassword);
            profile.Avatar = ProcessAvatar(updateProfileDto.AvatarData, profile.Avatar);
        }))!;

        return OperationResult.FromSuccess(updatedProfile);
    }


    private UserProfile PrepareProfile(RegisterProfileDto registerProfileDto)
    {
        return new UserProfile
        {
            User = new User
            {
                Email = registerProfileDto.Email,
                Password = _passwordHasher.Hash(registerProfileDto.Password)
            },
            Avatar = new Avatar
            {
                AvatarData = registerProfileDto.AvatarData
            },
            JoinDate = _timeService.Now(),
            FirstName = registerProfileDto.FirstName,
            LastName = registerProfileDto.LastName
        };
    }

    private string ProcessPassword(string oldPassword, string newPassword) =>
        string.IsNullOrEmpty(newPassword) ? oldPassword : _passwordHasher.Hash(newPassword);
    private static Avatar? ProcessAvatar(byte[] newAvatarData, Avatar? oldAvatar) =>
        newAvatarData.Any() == false ? oldAvatar : new() { AvatarData = newAvatarData };
    private static string GetNewValueOrExisting(string newValue, string existing) =>
        string.IsNullOrEmpty(newValue) == false ? newValue : existing;
}