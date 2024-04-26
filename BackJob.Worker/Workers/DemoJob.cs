using BackJob.Domain.Entries;
using BackJob.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace BackJob.Worker.Workers;


[DisallowConcurrentExecution]
[UnitOfWork]
public class DemoJob : IJob , ITransientDependency
{
    private readonly ILogger<DemoJob> _logger;
    private readonly IRepository<Product, int> _repository;
    private readonly IUnitOfWorkManager _uoOfWorkManager;

    public DemoJob(ILogger<DemoJob> logger, IRepository<Product, int> repository,IUnitOfWorkManager uoOfWorkManager)
    {
        _logger = logger;
        _repository = repository;
        _uoOfWorkManager = uoOfWorkManager;
    }

    /// <inheritdoc />
    public  async Task Execute(IJobExecutionContext context)
    {
        ValueContext.CurrentId.Value = "1";

        var query = await _repository.GetQueryableAsync();

        var list = await query.Where(x => x.Id > 0).ToListAsync();

        _logger.LogInformation("DemoJob is running. data is {@list}", list);
    }
}


