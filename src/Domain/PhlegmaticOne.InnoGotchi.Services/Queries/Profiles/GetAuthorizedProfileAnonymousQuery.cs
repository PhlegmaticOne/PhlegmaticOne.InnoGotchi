using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Queries.Profiles.Base;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.PasswordHasher;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Queries.Profiles;

public class GetAuthorizedProfileAnonymousQuery : IOperationResultQuery<AuthorizedProfileDto>
{
    public GetAuthorizedProfileAnonymousQuery(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }
    public string Password { get; }
}

public class GetAuthorizedProfileAnonymousQueryHandler :
    GetAuthorizedProfileQueryHandlerBase<GetAuthorizedProfileAnonymousQuery>
{
    private readonly IPasswordHasher _passwordHasher;

    public GetAuthorizedProfileAnonymousQueryHandler(IUnitOfWork unitOfWork,
        IJwtTokenGenerationService jwtTokenGenerationService,
        IPasswordHasher passwordHasher) :
        base(unitOfWork, jwtTokenGenerationService)
    {
        _passwordHasher = passwordHasher;
    }

    protected override Task<AuthorizedProfileDto?> GetAuthorizedProfileAsync(GetAuthorizedProfileAnonymousQuery request,
        CancellationToken cancellationToken)
    {
        var repository = UnitOfWork.GetRepository<UserProfile>();
        var password = _passwordHasher.Hash(request.Password);

        return repository.GetFirstOrDefaultAsync(
            predicate: p => p.User.Email == request.Email && p.User.Password == password,
            selector: s => new AuthorizedProfileDto
            {
                Email = s.User.Email,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Id = s.Id
            }, cancellationToken: cancellationToken);
    }
}