using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;

namespace PhlegmaticOne.InnoGotchi.Domain.Services;

public interface IInnoGotchiSignsUpdateService
{
    HungerLevel TryIncreaseHungerLevel(HungerLevel currentHungerLevel, DateTime lastFeedTime);
    ThirstyLevel TryIncreaseThirstLevel(ThirstyLevel currentThirstyLevel, DateTime lastDrinkTime);
    int TryIncreaseAge(int currentAge, DateTime lastAgeUpdatedTime);

    int CalculateHappinessDaysCount(HungerLevel currentHungerLevel, ThirstyLevel currentThirstyLevel,
        DateTime petCreationDate);

    bool IsDeadNow(HungerLevel currentHungerLevel, ThirstyLevel currentThirstyLevel, int age);
}