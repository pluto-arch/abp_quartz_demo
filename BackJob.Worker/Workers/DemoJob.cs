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
    private readonly BackJobDbContext _dbContext;
    private readonly IUnitOfWorkManager _uoOfWorkManager;

    public DemoJob(ILogger<DemoJob> logger, IRepository<Product, int> repository,BackJobDbContext dbContext,IUnitOfWorkManager uoOfWorkManager)
    {
        _logger = logger;
        _repository = repository;
        _dbContext = dbContext;
        _uoOfWorkManager = uoOfWorkManager;
    }

    #region this unitofwork attribute is not working
    /// <inheritdoc />
    [UnitOfWork]
    public virtual async Task Execute(IJobExecutionContext context)
    {
        ValueContext.CurrentId.Value = "1";

        var query = await _repository.GetQueryableAsync(); // abp repository. Based on the abp documentation, each method in the repository is considered as a uow. I don't understand why it is designed this way.

        var list = await query.Where(x => x.Id > 0).ToListAsync(); // this will throw exception dbcontext disposed

        _logger.LogInformation("DemoJob is running. data is {@list}", list);
    }
    #endregion


    #region this is working using  uoOfWorkManager.Begin

    ///// <inheritdoc />
    //public async Task Execute(IJobExecutionContext context)
    //{
    //    using (_uoOfWorkManager.Begin(requiresNew: true)) // this will work
    //    {
    //        ValueContext.CurrentId.Value = "1";

    //        var query = await _repository.GetQueryableAsync();

    //        var list = await query.Where(x => x.Id > 0).ToListAsync(); // worked

    //        _logger.LogInformation("DemoJob is running. data is {@list}", list);
    //    }
    //}

    #endregion


    #region this is working using dbcontext

    ///// <inheritdoc />
    //public async Task Execute(IJobExecutionContext context)
    //{
    //    ValueContext.CurrentId.Value = "1";

    //    var list = await _dbContext.Product.Where(x => x.Id > 0).ToListAsync(); // worked

    //    _logger.LogInformation("DemoJob is running. data is {@list}", list);
    //}

    #endregion
}