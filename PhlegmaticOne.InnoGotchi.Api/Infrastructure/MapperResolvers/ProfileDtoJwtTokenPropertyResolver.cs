using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.JwtToken;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.JwtTokensGeneration;
using PhlegmaticOne.JwtTokensGeneration.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;

public class ProfileDtoJwtTokenPropertyResolver : IValueResolver<UserProfile, AuthorizedProfileDto, JwtTokenDto>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public ProfileDtoJwtTokenPropertyResolver(IJwtTokenGenerator jwtTokenGenerator) => _jwtTokenGenerator = jwtTokenGenerator;

    public JwtTokenDto Resolve(UserProfile source, AuthorizedProfileDto destination, JwtTokenDto destMember, ResolutionContext context)
    {
        var userInfo = new UserRegisteringModel(source.Id, source.FirstName, source.LastName, source.User.Email);
        var tokenValue = _jwtTokenGenerator.GenerateToken(userInfo);
        return new JwtTokenDto(tokenValue);
    }
}