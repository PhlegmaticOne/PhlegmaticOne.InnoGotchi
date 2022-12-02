using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCrontab;

namespace PhlegmaticOne.HostedServices;

public abstract class CrontabScheduledHostedService : ScopedHostedService
{
    private DateTime _nextRun;
    private CrontabSchedule? _schedule;

    protected CrontabScheduledHostedService(IServiceScopeFactory serviceScopeFactory, ILogger logger) :
        base(serviceScopeFactory, logger)
    {
        GetSchedule();
    }

    protected abstract string Schedule { get; }

    protected virtual bool IsExecuteOnServerRestart => false;
    protected virtual bool IncludingSeconds => false;

    private void GetSchedule()
    {
        if (string.IsNullOrEmpty(Schedule)) throw new ArgumentException(nameof(Schedule));

        _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = IncludingSeconds
        });

        var currentDateTime = DateTime.Now;
        if (IsExecuteOnServerRestart)
        {
            _nextRun = currentDateTime.AddMilliseconds(ProcessTime);
            Logger.LogInformation(
                $"{GetType().Name} ({nameof(IsExecuteOnServerRestart)} = {IsExecuteOnServerRestart})");
        }
        else
        {
            _nextRun = _schedule.GetNextOccurrence(currentDateTime);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        do
        {
            var now = DateTime.Now;
            if (now > _nextRun)
            {
                await ProcessAsync(token);
                _nextRun = _schedule!.GetNextOccurrence(DateTime.Now);
            }

            await Task.Delay(ProcessTime, token);
        } while (!token.IsCancellationRequested);
    }
}