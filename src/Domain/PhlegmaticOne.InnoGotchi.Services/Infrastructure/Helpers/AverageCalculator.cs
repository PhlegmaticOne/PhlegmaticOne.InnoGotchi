namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.Helpers;

public static class AverageCalculator
{
    public static TimeSpan CalculateNewAverage(TimeSpan currentAverage, DateTime lastActionTime, DateTime now, int currentActionsCount)
    {
        var difference = now - lastActionTime;
        return (currentAverage + difference) / (currentActionsCount + 1);
    }
}