using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace BackJob.Worker
{
    [DependsOn(
        typeof(AbpQuartzModule)
    )]
    public class BackJobWorkerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            context.Services.AddSingleton<IJobFactory, LineJobFactory>();
            context.Services.AddSingleton<JobRunner>();
        }


        /// <inheritdoc />
        public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            await base.OnPostApplicationInitializationAsync(context);
            var runner = context.ServiceProvider.GetRequiredService<JobRunner>();
            await runner.StartAsync();
        }
    }
}
