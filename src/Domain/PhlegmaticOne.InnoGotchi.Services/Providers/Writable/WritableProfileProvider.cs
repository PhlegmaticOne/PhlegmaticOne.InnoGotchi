using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableProfileProvider : IWritableProfilesProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITimeService _timeService;

    public WritableProfileProvider(IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher, 
        ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _timeService = timeService;
    }
    public async Task<UserProfile> CreateAsync(RegisterProfileDto registerProfileDto,
        CancellationToken cancellationToken = new())
    {
        var prepared = PrepareProfile(registerProfileDto);
        var repository = _unitOfWork.GetRepository<UserProfile>();
        var createdProfile = await repository.CreateAsync(prepared, cancellationToken);
        return createdProfile;
    }

    public async Task<UserProfile> UpdateAsync(Guid profileId, UpdateProfileDto updateProfileDto,
        CancellationToken cancellationToken = new())
    {
        var repository = _unitOfWork.GetRepository<UserProfile>();
        var profile = await repository.GetByIdOrDefaultAsync(profileId,
            include: i => i.Include(x => x.User), cancellationToken: cancellationToken);
        var updatedProfile = (await repository.UpdateAsync(profile!, updating =>
        {
            updating.FirstName = GetNewValueOrExisting(updateProfileDto.FirstName, profile!.FirstName);
            updating.LastName = GetNewValueOrExisting(updateProfileDto.LastName, profile.LastName);
            updating.User.Password = ProcessPassword(profile.User.Password, updateProfileDto.NewPassword);
            updating.Avatar = ProcessAvatar(updateProfileDto.AvatarData, profile.Avatar);
        }, cancellationToken))!;

        return updatedProfile;
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