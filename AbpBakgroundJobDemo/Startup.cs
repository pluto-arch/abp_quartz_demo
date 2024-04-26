

using BackJob.Domain.Entries;
using BackJob.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace AbpBakgroundJobDemo;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration  configuration)
    {
        _configuration = configuration;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<BackJobHostModule>();
        services.AddRouting();
        services.AddHealthChecks();
    }



    public void Configure(IApplicationBuilder app)
    {
        app.InitializeApplicationAsync().Wait();
        app.UseRouting();
        app.UseUnitOfWork();
        app.UseAbpSerilogEnrichers();
        app.UseEndpoints(endpoints=>
        {
            //endpoints.MapGet("/a", async ([FromServices]IRepository<Product,int> repository)=>
            //{
            //    ValueContext.CurrentId.Value = "1";
            //    var data= await repository.InsertAsync(new Product
            //    {
            //        Name = "hello",
            //        Address = "蛋疼abp"
            //    });

            //    return Results.Ok(data);
            //});

            endpoints.MapGet("/a", async ()=>
            {
                return Results.Ok("123");
            });
        });

    }
}