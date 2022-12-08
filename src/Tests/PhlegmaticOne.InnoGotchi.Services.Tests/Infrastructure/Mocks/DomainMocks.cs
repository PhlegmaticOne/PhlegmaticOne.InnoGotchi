using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Shared.JwtToken;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.PasswordHasher;

namespace PhlegmaticOne.InnoGotchi.Services.Tests.Infrastructure.Mocks;

internal static class DomainMocks
{
    public static IMapper DomainMapper() =>
        new MapperConfiguration(x =>
        {
            x.AddProfile(new ProfileMapperConfiguration());
            x.AddProfile(new FarmMapperConfiguration());
            x.AddProfile(new InnoGotchiComponentsMapperConfiguration());
            x.AddProfile(new InnoGotchiesMapperConfiguration());
        }).CreateMapper();

    public static IReadableInnoGotchiProvider ReadablePetProviderWithPetToReturn(InnoGotchiModel pet)
    {
        var readableProvider = new Mock<IReadableInnoGotchiProvider>();
        readableProvider.Setup(x => x.GetDetailedAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pet);
        return readableProvider.Object;
    }

    public static IValidator<T> AlwaysTrueValidator<T>()
    {
        var validator = new Mock<IValidator<T>>();
        validator.Setup(x => x.ValidateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        return validator.Object;
    }

    public static IPasswordHasher PasswordHasherEmptyMock()
    {
        var passwordHasher = new Mock<IPasswordHasher>();
        passwordHasher.Setup(x => x.Hash(It.IsAny<string>()))
            .Returns((string password) => password);
        return passwordHasher.Object;
    }

    public static IJwtTokenGenerationService JwtGenerationReturningValueMock(string jwtValue)
    {
        var jwtTokenGenerationService = new Mock<IJwtTokenGenerationService>();
        jwtTokenGenerationService.Setup(x => x.GenerateJwtToken(It.IsAny<AuthorizedProfileDto>()))
            .Returns(new JwtTokenDto(jwtValue));
        return jwtTokenGenerationService.Object;
    }
}