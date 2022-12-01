using FluentValidation;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Services.Commands.Collaborations;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Validators;

public class CreateCollaborationValidator : AbstractValidator<CreateCollaborationCommand>
{
    public CreateCollaborationValidator(IUnitOfWork unitOfWork)
    {
        var collaborationsRepository = unitOfWork.GetRepository<Collaboration>();
        var profilesRepository = unitOfWork.GetRepository<UserProfile>();
        var farmsRepository = unitOfWork.GetRepository<Farm>();

        RuleFor(x => x.ToProfileId)
            .MustAsync((id, ct) => profilesRepository.ExistsAsync(x => x.Id == id, ct))
            .WithMessage("User Profile doesn't exist");

        RuleFor(x => x.ProfileId)
            .MustAsync((id, ct) => farmsRepository.ExistsAsync(x => x.OwnerId == id, ct))
            .WithMessage("You must create your Farm to make collaborations");

        RuleFor(x => x)
            .MustAsync((model, ct) => collaborationsRepository
                .AllAsync(x => x.Farm.OwnerId != model.ProfileId && x.UserProfileId != model.ToProfileId, ct))
            .WithMessage("You already have a collaboration with this profile");
    }
}