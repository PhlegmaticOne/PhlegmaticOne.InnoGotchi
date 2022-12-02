using PhlegmaticOne.InnoGotchi.Domain.Services;

namespace PhlegmaticOne.InnoGotchi.Services.Services;

public class TimeService : ITimeService
{
    private DateTime _dateTime;

    public DateTime Now()
    {
        if (_dateTime == DateTime.MinValue) _dateTime = DateTime.UtcNow;
        return _dateTime;
    }
}