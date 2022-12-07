namespace PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

public static class AppErrorMessages
{
    public const string FarmDoesNotExistMessage = "You must create your farm to do any operations";
    public const string ProfileDoesNotExistMessage = "Profile does not exist";
    public const string HaveNoFarmForCollaborationMessage = "You have no farm and you can't create collaborations without it";
    public const string SuchCollaborationExistsMessage = "You already have a collaboration with this profile";
    public const string AlreadyHaveFarmMessage = "You already have a farm";
    public const string FarmNameReservedMessage = "Farm name already reserved";
    public const string NameIsTooShortMessage = "Name is too short";
    public const string NameIsTooLongMessage = "Name is too long";
    public const string InnoGotchiNameReservedMessage = "InnoGotchi name already reserved";
    public const string InnoGotchiMustHaveBodyMessage = "InnoGotchi must have a body";
    public const string UnknownComponentCategoryNameMessage = "Unknown component category";
    public const string UnknownComponentImageUrlMessage = "Unknown component image url";
    public const string CannotGetFarmBecauseOfCollaboration = "You can't get information about this farm, because it is not yours and not one of collaborated";
    public const string PetDoesNotBelongToProfileMessage = "You can't make any actions with this InnoGotchi, because it is not yours or it is not from collaborated farms";
    public const string EmailExistsMessage = "Unable to create user profile. User with email exist";
    public const string EmailIncorrectMessage = "Email specified is not correct";
    public const string PasswordIncorrectMessage = "Password specified is not correct";
    public const string CannotUpdateDeadPetMessage = "You can't update dead pet";
    public const string OldPasswordIsIncorrectMessage = "Old password is incorrect";
    public const string ProfileDoesNotHaveFarmStatistics = "Profile does not have farm statistics. You must create your farm";
    public const string UnknownComponentMessage = "Unknown component for creating InnoGotchi";
    public const string PetDoesNotExistMessage = "Pet doesn't exist";
}