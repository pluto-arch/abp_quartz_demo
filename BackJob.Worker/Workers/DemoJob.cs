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

    #region this unitofwork attribute is not working
    ///// <inheritdoc />
    //[UnitOfWork]
    //public  async Task Execute(IJobExecutionContext context)
    //{
    //    ValueContext.CurrentId.Value = "1";

    //    var query = await _repository.GetQueryableAsync();

    //    var list = await query.Where(x => x.Id > 0).ToListAsync(); // this will throw exception dbcontext disposed

    //    _logger.LogInformation("DemoJob is running. data is {@list}", list);
    //}
    #endregion


    #region this is working

    /// <inheritdoc />
    public  async Task Execute(IJobExecutionContext context)
    {
        using (_uoOfWorkManager.Begin(requiresNew:true)) // this will work
        {
            ValueContext.CurrentId.Value = "1";

            var query = await _repository.GetQueryableAsync();

            var list = await query.Where(x => x.Id > 0).ToListAsync(); // 

            _logger.LogInformation("DemoJob is running. data is {@list}", list);
        }
    }

    #endregion
    
}