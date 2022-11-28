using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class CreateCollaborationValidator : AbstractValidator<IdentityModel<CreateCollaborationDto>>
{
    public CreateCollaborationValidator(IUnitOfWork unitOfWork)
    {
        var collaborationsRepository = unitOfWork.GetRepository<Collaboration>();
        var profilesRepository = unitOfWork.GetRepository<UserProfile>();
        var farmsRepository = unitOfWork.GetRepository<Farm>();

        RuleFor(x => x.Entity.Id)
            .MustAsync((id, _) => profilesRepository.ExistsAsync(x => x.Id == id))
            .WithMessage(id => $"User Profile {id} doesn't exist");
            
        RuleFor(x => x.ProfileId)
            .MustAsync((id, _) => farmsRepository.ExistsAsync(x => x.OwnerId == id))
            .WithMessage("You must create your Farm to make collaborations");

        RuleFor(x => x)
            .MustAsync((model, _) => 
                collaborationsRepository.AllAsync(x => x.Farm.OwnerId != model.ProfileId && x.UserProfileId != model.Entity.Id))
            .WithMessage(model => $"You already have a collaboration with Profile {model.Entity.Id}");
    }
}