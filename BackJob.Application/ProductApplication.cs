using BackJob.Domain.Entries;
using BackJob.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace BackJob.Application;

public class ProductApplication : ApplicationService , ITransientDependency
{
    private readonly IRepository<Product, int> _repository;

    public ProductApplication(IRepository<Product,int> repository)
    {
        _repository = repository;
    }


    [UnitOfWork]
    public async Task<List<Product>> DeNameAsync()
    {

        ValueContext.CurrentId.Value = "1";
        var query = await _repository.GetQueryableAsync();
        var list = await query.Where(x => x.Id > 0).ToListAsync();
        return list;
    }
}