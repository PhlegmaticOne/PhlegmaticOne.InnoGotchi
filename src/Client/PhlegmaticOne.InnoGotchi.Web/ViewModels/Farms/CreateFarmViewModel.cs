using PhlegmaticOne.InnoGotchi.Web.ViewModels.Base;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Farms;

public class CreateFarmViewModel : ErrorHavingViewModel
{
    public string Name { get; set; } = null!;
}