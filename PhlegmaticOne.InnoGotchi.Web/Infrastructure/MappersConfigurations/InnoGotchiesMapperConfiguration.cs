﻿using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class InnoGotchiesMapperConfiguration : Profile
{
    public InnoGotchiesMapperConfiguration()
    {
        CreateMap<InnoGotchiModelComponentDto, InnoGotchiComponentViewModel>();
        CreateMap<InnoGotchiDto, InnoGotchiViewModel>();
    }
}