﻿using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.PagedLists;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;

public class InnoGotchiesMappingConfiguration : Profile
{
    public InnoGotchiesMappingConfiguration()
    {
        CreateMap<InnoGotchiModel, InnoGotchiDtoBase>()
            .ForMember(x => x.HungerLevel, o => o.MapFrom(y => y.HungerLevel.ToString()))
            .ForMember(x => x.ThirstyLevel, o => o.MapFrom(y => y.ThirstyLevel.ToString()))
            .ForMember(x => x.IsDead, o => o.MapFrom(x => DeathDateNotMin(x.DeadSince)))
            .ForMember(x => x.IsFeedingAllowable,
                o => o.MapFrom(x => DeathDateNotMin(x.DeadSince) == false && x.HungerLevel != HungerLevel.Full))
            .ForMember(x => x.IsDrinkingAllowable, 
                o => o.MapFrom(x => DeathDateNotMin(x.DeadSince) == false && x.ThirstyLevel != ThirstyLevel.Full))
            .ForMember(x => x.IsNewBorn, o => o.MapFrom(x => x.Age == 0));

        CreateMap<InnoGotchiModel, PreviewInnoGotchiDto>()
            .IncludeBase<InnoGotchiModel, InnoGotchiDtoBase>();

        CreateMap<InnoGotchiModel, DetailedInnoGotchiDto>()
            .IncludeBase<InnoGotchiModel, InnoGotchiDtoBase>();

        CreateMap<InnoGotchiModel, ReadonlyInnoGotchiPreviewDto>()
            .ForMember(x => x.HungerLevel, o => o.MapFrom(x => x.HungerLevel.ToString()))
            .ForMember(x => x.ThirstyLevel, o => o.MapFrom(x => x.ThirstyLevel.ToString()))
            .ForMember(x => x.IsDead, o => o.MapFrom(x => DeathDateNotMin(x.DeadSince)))
            .ForMember(x => x.IsNewBorn, o => o.MapFrom(x => x.Age == 0))
            .ForMember(x => x.ProfileFirstName, o => o.MapFrom(x => x.Farm.Owner.FirstName))
            .ForMember(x => x.ProfileFarmName, o => o.MapFrom(x => x.Farm.Name))
            .ForMember(x => x.ProfileLastName, o => o.MapFrom(x => x.Farm.Owner.LastName));

        CreateMap<PagedList<InnoGotchiModel>, PagedList<ReadonlyInnoGotchiPreviewDto>>();
    }

    private static bool DeathDateNotMin(DateTime deathDate) => deathDate != DateTime.MinValue;
}