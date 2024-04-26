using Volo.Abp.Application;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace BackJob.Application
{

    [DependsOn(typeof(AbpDddApplicationModule),typeof(AbpAutofacModule))]
    public class BackJobApplicationModule: AbpModule
    {

    }
}
