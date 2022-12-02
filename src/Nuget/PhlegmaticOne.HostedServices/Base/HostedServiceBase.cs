using Microsoft.Extensions.Hosting;

namespace PhlegmaticOne.HostedServices.Base;

public abstract class HostedServiceBase : IHostedService
{
    protected const int ProcessTime = 5000;
    private readonly CancellationTokenSource _stoppingCancellationTokenSource = new();
    private Task? _executingTask;

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        _executingTask = ExecuteAsync(_stoppingCancellationTokenSource.Token);
        return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_executingTask == null) return;

        try
        {
            _stoppingCancellationTokenSource.Cancel();
        }
        finally
        {
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }

    protected virtual async Task ExecuteAsync(CancellationToken token)
    {
        do
        {
            await ProcessAsync(token);
            await Task.Delay(ProcessTime, token);
        } while (!token.IsCancellationRequested);
    }

    protected abstract Task ProcessAsync(CancellationToken token);
}