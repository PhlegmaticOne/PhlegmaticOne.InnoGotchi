using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;
using PhlegmaticOne.JwtTokensGeneration;
using PhlegmaticOne.JwtTokensGeneration.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;

public class ProfileDtoJwtTokenPropertyResolver : IValueResolver<UserProfile, ProfileDto, JwtTokenDto>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public ProfileDtoJwtTokenPropertyResolver(IJwtTokenGenerator jwtTokenGenerator) => _jwtTokenGenerator = jwtTokenGenerator;

    public JwtTokenDto Resolve(UserProfile source, ProfileDto destination, JwtTokenDto destMember, ResolutionContext context)
    {
        var userInfo = new UserRegisteringModel(source.Id, source.FirstName, source.SecondName, source.User.Email);
        var tokenValue = _jwtTokenGenerator.GenerateToken(userInfo);
        return new JwtTokenDto(tokenValue);
    }
}