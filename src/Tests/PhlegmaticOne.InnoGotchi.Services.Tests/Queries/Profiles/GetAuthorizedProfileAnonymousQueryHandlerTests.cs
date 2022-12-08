using FluentAssertions;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Queries.Profiles;
using PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Queries.Profiles;

public class GetAuthorizedProfileAnonymousQueryHandlerTests
{
    private readonly GetAuthorizedProfileAnonymousQueryHandler _sut;
    private readonly string JwtValue = "JWT";

    private UserProfile _model;
    public GetAuthorizedProfileAnonymousQueryHandlerTests()
    {
        var data = UnitOfWorkMock.Create();
        var passwordHasher = DomainMocks.PasswordHasherEmptyMock();
        var jwtTokenGenerationService = DomainMocks.JwtGenerationReturningValueMock(JwtValue);

        _model = data.ThatHasFarm;
        _sut = new GetAuthorizedProfileAnonymousQueryHandler(data.UnitOfWork, jwtTokenGenerationService, passwordHasher);
    }

    [Fact]
    public async Task ShouldReturnFailedResult_BecauseProfileDoesNotExist_Test()
    {
        var profileEmail = Guid.NewGuid().ToString();
        var profilePassword = Guid.NewGuid().ToString();
        var query = new GetAuthorizedProfileAnonymousQuery(profileEmail, profilePassword);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNull().And.Be(AppErrorMessages.ProfileDoesNotExistMessage);
    }

    [Fact]
    public async Task ShouldReturnSuccessful_Test()
    {
        var profileEmail = _model.User.Email;
        var profilePassword = _model.User.Password;
        var query = new GetAuthorizedProfileAnonymousQuery(profileEmail, profilePassword);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result!.JwtToken.Token.Should().Be(JwtValue);
    }
}