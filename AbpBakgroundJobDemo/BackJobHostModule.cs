using BackJob.Domain;
using BackJob.EntityFrameworkCore;
using BackJob.Worker;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpBakgroundJobDemo;


[DependsOn(
    typeof(BackJobDomainModule),
    typeof(BackJobEntityFrameworkCoreModule),
    typeof(BackJobWorkerModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAutofacModule)
)]
public class BackJobHostModule : AbpModule
{
    
}