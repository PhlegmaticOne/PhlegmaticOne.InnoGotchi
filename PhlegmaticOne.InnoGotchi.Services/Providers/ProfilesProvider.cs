using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.PasswordHasher.Base;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers;

public class ProfilesProvider : IProfilesProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public ProfilesProvider(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }
    public async Task<OperationResult<UserProfile>> CreateAsync(RegisterProfileDto registerProfileDto)
    {
        var prepared = PrepareProfile(registerProfileDto);

        //await using var transaction = await _unitOfWork.BeginTransactionAsync();
        UserProfile createdProfile;

        try
        {
            var repository = _unitOfWork.GetDataRepository<UserProfile>();
            createdProfile = await repository.CreateAsync(prepared);
            await _unitOfWork.SaveChangesAsync();
            //await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            //await transaction.RollbackAsync();
            return OperationResult.FromFail<UserProfile>(e.Message);
        }

        return OperationResult.FromSuccess(createdProfile);
    }

    public async Task<OperationResult<UserProfile>> GetExistingOrDefaultAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<UserProfile>();
        var profile = await repository.GetFirstOrDefaultAsync(WithId(profileId), IncludeUser());

        if (profile is null)
        {
            var notExistingUserErrorMessage = $"There is no user with id: {profileId}";
            return OperationResult.FromFail<UserProfile>(notExistingUserErrorMessage);
        }

        return OperationResult.FromSuccess(profile);
    }

    public async Task<OperationResult<UserProfile>> GetExistingOrDefaultAsync(string email, string password)
    {
        var repository = _unitOfWork.GetDataRepository<UserProfile>();
        var profile = await repository.GetFirstOrDefaultAsync(WithEmail(email), IncludeUser());

        if (profile is null)
        {
            var notExistingUserErrorMessage = $"There is no user with email: {email}";
            return OperationResult.FromFail<UserProfile>(notExistingUserErrorMessage);
        }

        if (PasswordsAreEqual(password, profile.User.Password) == false)
        {
            const string incorrectPasswordMessage = "You've entered incorrect password";
            return OperationResult.FromFail<UserProfile>(incorrectPasswordMessage);
        }

        return OperationResult.FromSuccess(profile);
    }

    public async Task<OperationResult<UserProfile>> UpdateAsync(UpdateProfileDto updateProfileDto)
    {
        var repository = _unitOfWork.GetDataRepository<UserProfile>();

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        UserProfile updatedProfile;

        try
        {
            updatedProfile = (await repository.UpdateAsync(updateProfileDto.Id, profile =>
            {
                profile.FirstName = GetNewValueOrExisting(updateProfileDto.FirstName, profile.FirstName);
                profile.LastName = GetNewValueOrExisting(updateProfileDto.LastName, profile.LastName);
                profile.User.Password = ProcessPassword(profile.User.Password, updateProfileDto.NewPassword);
                profile.Avatar = ProcessAvatar(updateProfileDto.AvatarData, profile.Avatar);
            }))!;
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return OperationResult.FromFail<UserProfile>(e.Message);
        }

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
            JoinDate = DateTime.UtcNow,
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
    private bool PasswordsAreEqual(string firstPassword, string secondPassword) =>
        _passwordHasher.Verify(firstPassword, secondPassword);
    private static Func<IQueryable<UserProfile>, IIncludableQueryable<UserProfile, object>> IncludeUser() =>
        i => i.Include(x => x.User);
    private static Expression<Func<UserProfile, bool>> WithEmail(string email) =>
        u => u.User.Email == email;
    private static Expression<Func<UserProfile, bool>> WithId(Guid profileId) =>
        u => u.Id == profileId;
}