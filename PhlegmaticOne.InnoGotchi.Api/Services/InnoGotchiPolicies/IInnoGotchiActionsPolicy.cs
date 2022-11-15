namespace PhlegmaticOne.InnoGotchi.Api.Services.InnoGotchiPolicies;

public interface IInnoGotchiActionsPolicy
{
    public TimeSpan TimeToIncreaseHungerLevel { get; }
    public TimeSpan TimeToIncreaseThirstLevel { get; }
    public TimeSpan TimeToIncreaseAge { get; }
}