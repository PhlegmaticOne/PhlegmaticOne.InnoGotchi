namespace PhlegmaticOne.InnoGotchi.Api.Services.InnoGotchiPolicies;

public class InnoGotchiActionsPolicy : IInnoGotchiActionsPolicy
{
    public InnoGotchiActionsPolicy(TimeSpan timeToIncreaseHungerLevel, TimeSpan timeToIncreaseThirstLevel, TimeSpan timeToIncreaseAge)
    {
        TimeToIncreaseHungerLevel = timeToIncreaseHungerLevel;
        TimeToIncreaseAge = timeToIncreaseAge;
        TimeToIncreaseThirstLevel = timeToIncreaseThirstLevel;
    }

    public TimeSpan TimeToIncreaseHungerLevel { get; }
    public TimeSpan TimeToIncreaseThirstLevel { get; }
    public TimeSpan TimeToIncreaseAge { get; }
}