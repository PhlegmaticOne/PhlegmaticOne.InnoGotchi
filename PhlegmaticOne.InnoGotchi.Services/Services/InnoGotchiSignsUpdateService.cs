using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Services.Helpers;

namespace PhlegmaticOne.InnoGotchi.Services.Services;

public class InnoGotchiSignsUpdateService : IInnoGotchiSignsUpdateService
{
    private readonly TimeSpan _timeToIncreaseHungerLevel;
    private readonly TimeSpan _timeToIncreaseThirstLevel;
    private readonly TimeSpan _timeToIncreaseAge;

    public InnoGotchiSignsUpdateService(TimeSpan timeToIncreaseHungerLevel, 
        TimeSpan timeToIncreaseThirstLevel,
        TimeSpan timeToIncreaseAge)
    {
        _timeToIncreaseHungerLevel = timeToIncreaseHungerLevel;
        _timeToIncreaseThirstLevel = timeToIncreaseThirstLevel;
        _timeToIncreaseAge = timeToIncreaseAge;
    }

    public HungerLevel TryIncreaseHungerLevel(HungerLevel currentHungerLevel, DateTime lastFeedTime)
    {
        var now = DateTime.Now;
        return SynchronizationHelper.SynchronizeEnumWithTime(currentHungerLevel, now, lastFeedTime, _timeToIncreaseHungerLevel);
    }

    public ThirstyLevel TryIncreaseThirstLevel(ThirstyLevel currentThirstyLevel, DateTime lastDrinkTime)
    {
        var now = DateTime.Now;
        return SynchronizationHelper.SynchronizeEnumWithTime(currentThirstyLevel, now, lastDrinkTime, _timeToIncreaseThirstLevel);
    }

    public int TryIncreaseAge(int currentAge, DateTime lastAgeUpdatedTime)
    {
        var now = DateTime.Now;
        return SynchronizationHelper.IncreaseUntilNotSynchronizedWithTime(currentAge, now, lastAgeUpdatedTime,
            _timeToIncreaseAge);
    }

    public int CalculateHappinessDaysCount(HungerLevel currentHungerLevel, ThirstyLevel currentThirstyLevel,
        DateTime petCreationDate)
    {
        var now = DateTime.Now;

        if ((int)currentHungerLevel > (int)HungerLevel.Normal || (int)currentThirstyLevel > (int)ThirstyLevel.Normal)
        {
            return 0;
        }

        return (int)(now - petCreationDate).TotalDays;
    }

    public bool IsDeadNow(HungerLevel currentHungerLevel, ThirstyLevel currentThirstyLevel, int age)
    {
        return currentThirstyLevel == ThirstyLevel.Dead || currentHungerLevel == HungerLevel.Dead;
    }
}