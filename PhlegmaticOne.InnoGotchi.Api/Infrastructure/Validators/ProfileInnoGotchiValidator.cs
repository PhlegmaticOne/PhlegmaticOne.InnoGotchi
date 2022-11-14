using FluentValidation;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Validators;

public class ProfileInnoGotchiValidator : AbstractValidator<IdentityInnoGotchiModel>
{
    public ProfileInnoGotchiValidator(IDataService dataService, IServerAddressProvider serverAddressProvider)
    {
        var componentsRepository = dataService.GetDataRepository<InnoGotchiComponent>();

        RuleFor(x => x.Components)
            .MustAsync(async (components, _) =>
            {
                var serverUri = serverAddressProvider.ServerAddressUri;
                var validPaths = await componentsRepository.GetAllAsync(x => serverUri.Combine(x.ImageUrl).AbsoluteUri);
                return components.All(x => validPaths.Contains(x.ImageUrl));
            })
            .WithMessage("Component paths are incorrect");
    }
}