namespace PhlegmaticOne.InnoGotchi.Api.Services.Time;

public class TimeProvider : ITimeProvider
{
    public DateTime Now() => DateTime.Now;
}