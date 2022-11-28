using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
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

    public async Task<UserProfile?> GetExistingOrDefaultAsync(string email, string password)
    {
        var repository = _unitOfWork.GetRepository<UserProfile>();
        var passwordHash = _passwordHasher.Hash(password);
        return await repository.GetFirstOrDefaultAsync(WithEmailAndPassword(email, passwordHash), IncludeUser());
    }

    public async Task<UserProfile?> GetExistingOrDefaultAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetRepository<UserProfile>();
        return await repository.GetFirstOrDefaultAsync(WithId(profileId), IncludeUser());
    }

    private static Func<IQueryable<UserProfile>, IIncludableQueryable<UserProfile, object>> IncludeUser() =>
        i => i.Include(x => x.User);
    private static Expression<Func<UserProfile, bool>> WithEmailAndPassword(string email, string password) =>
        u => u.User.Email == email && u.User.Password == password;
    private static Expression<Func<UserProfile, bool>> WithId(Guid profileId) =>
        u => u.Id == profileId;
}