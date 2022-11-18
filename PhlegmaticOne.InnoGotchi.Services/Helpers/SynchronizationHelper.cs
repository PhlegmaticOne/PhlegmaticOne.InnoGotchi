namespace PhlegmaticOne.InnoGotchi.Services.Helpers;

internal static class SynchronizationHelper
{
    public static int IncreaseUntilNotSynchronizedWithTime(int valueToIncrease, DateTime currentTime,
        DateTime lastActionTime, TimeSpan timeToIncrease)
    {
        var nonActionPeriod = currentTime - lastActionTime;

        while (nonActionPeriod >= timeToIncrease)
        {
            ++valueToIncrease;
            nonActionPeriod -= timeToIncrease;
        }

        return valueToIncrease;
    }

    public static T SynchronizeEnumWithTime<T>(T enumValue, DateTime currentTime,
        DateTime lastActionTime, TimeSpan timeToIncrease) where T : struct, Enum
    {
        var currentEnumValue = Convert.ToInt32(enumValue);
        var newEnumValue = IncreaseUntilNotSynchronizedWithTime(currentEnumValue, currentTime, lastActionTime, timeToIncrease);
        return EnumHelper.EnumValueOrMax<T>(newEnumValue);
    }
}
