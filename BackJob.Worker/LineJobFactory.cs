using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Quartz.Spi;
using Quartz;
using System.Collections.Concurrent;
using static Quartz.Logging.OperationName;

namespace BackJob.Worker;

public class LineJobFactory : IJobFactory
{
    private readonly IServiceScopeFactory _serviceProvider;
    private readonly ILogger<LineJobFactory> _logger;
    protected readonly ConcurrentDictionary<IJob, IServiceScope> _scopes = new ConcurrentDictionary<IJob, IServiceScope>();
    public LineJobFactory(IServiceScopeFactory serviceProvider, ILogger<LineJobFactory> logger = null)
    {
        _serviceProvider = serviceProvider;
        _logger = logger ?? NullLogger<LineJobFactory>.Instance;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var scope = _serviceProvider.CreateScope();
        IJob job;

        try
        {
            job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }
        catch
        {
            scope.Dispose();
            throw;
        }

        if (!_scopes.TryAdd(job, scope))
        {
            scope.Dispose();
            throw new Exception("Failed to track DI scope");
        }
        return job;
    }

    public void ReturnJob(IJob job)
    {
        if (_scopes.TryRemove(job, out var scope))
        {
            scope.Dispose();
        }
        (job as IDisposable)?.Dispose();
    }
}