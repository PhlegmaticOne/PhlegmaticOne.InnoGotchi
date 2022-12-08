using AutoMapper;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.ValueConverters;

public class TimeToLocalZoneConverter : IValueConverter<DateTime, DateTime>
{
    public DateTime Convert(DateTime sourceMember, ResolutionContext context) =>
        TimeZoneInfo.ConvertTimeFromUtc(sourceMember, TimeZoneInfo.Local);
}