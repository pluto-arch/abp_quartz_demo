using BackJob.Domain.Entries;
using BackJob.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace BackJob.Worker.Workers;


[DisallowConcurrentExecution]
public class DemoJob :  IJob ,ITransientDependency
{
    private readonly ILogger<DemoJob> _logger;
    private readonly IRepository<Product, int> _repository;

    public DemoJob(ILogger<DemoJob> logger, IRepository<Product, int> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    /// <inheritdoc />
    [UnitOfWork]
    public virtual async Task Execute(IJobExecutionContext context)
    {
        ValueContext.CurrentId.Value = "1";

        var query = await _repository.GetQueryableAsync(); // abp repository. Based on the abp documentation, each method in the repository is considered as a uow. I don't understand why it is designed this way.

        var list = await query.Where(x => x.Id > 0).ToListAsync(); // this will throw exception dbcontext disposed

        _logger.LogInformation("DemoJob is running. data is {@list}", list);
    }
}