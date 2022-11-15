namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;

internal static class EnumHelper
{
    public static T EnumValueOrMax<T>(int currentEnumValue) where T : struct, Enum
    {
        var maxEnumValue = Enum.GetValues(typeof(T)).Cast<int>().Max();
        var enumValue = currentEnumValue > maxEnumValue ? maxEnumValue : currentEnumValue;
        return (T)Enum.ToObject(typeof(T), enumValue);
    }
}