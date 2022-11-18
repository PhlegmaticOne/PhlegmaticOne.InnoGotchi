using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;
using PhlegmaticOne.PasswordHasher.Base;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableProfileProvider : IReadableProfileProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public ReadableProfileProvider(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
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

    private bool PasswordsAreEqual(string firstPassword, string secondPassword) =>
        _passwordHasher.Verify(firstPassword, secondPassword);
    private static Func<IQueryable<UserProfile>, IIncludableQueryable<UserProfile, object>> IncludeUser() =>
        i => i.Include(x => x.User);
    private static Expression<Func<UserProfile, bool>> WithEmail(string email) =>
        u => u.User.Email == email;
    private static Expression<Func<UserProfile, bool>> WithId(Guid profileId) =>
        u => u.Id == profileId;
}