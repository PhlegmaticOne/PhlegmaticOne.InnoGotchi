﻿using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Farms;

public class DetailedFarmViewModel
{
    public string Name { get; set; } = null!;
    public List<PreviewInnoGotchiViewModel> InnoGotchies { get; set; } = new();
}