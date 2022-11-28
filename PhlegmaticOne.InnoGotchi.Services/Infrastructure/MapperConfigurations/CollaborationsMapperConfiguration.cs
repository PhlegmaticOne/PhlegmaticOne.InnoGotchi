using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;

public class CollaborationsMapperConfiguration : Profile
{
    public CollaborationsMapperConfiguration()
    {
        CreateMap<Collaboration, CollaborationDto>()
            .ForMember(x => x.CollaboratorEmail, o => o.MapFrom(x => x.Collaborator.User.Email))
            .ForMember(x => x.CollaboratorFirstName, o => o.MapFrom(x => x.Collaborator.FirstName))
            .ForMember(x => x.CollaboratorLastName, o => o.MapFrom(x => x.Collaborator.LastName))
            .ForMember(x => x.FarmName, o => o.MapFrom(x => x.Farm.Name));
    }
}