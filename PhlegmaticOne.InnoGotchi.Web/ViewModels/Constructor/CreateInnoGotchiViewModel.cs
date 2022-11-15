using PhlegmaticOne.InnoGotchi.Web.ViewModels.Base;

namespace PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

public class CreateInnoGotchiViewModel : ErrorHavingViewModel
{
    public string Name { get; set; } = null!;
    public List<CreateInnoGotchiComponentViewModel> Components { get; set; } = null!;
}