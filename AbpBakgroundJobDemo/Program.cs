using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace AbpBakgroundJobDemo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Volo", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.Console())
                .CreateLogger();



            try
            {
                Log.Information("Starting web host.");
                var builder = Host.CreateDefaultBuilder(args);
                builder.ConfigureWebHostDefaults(op=>
                {
                    op.UseStartup<Startup>()
                        .UseKestrel()
                        .CaptureStartupErrors(true);
                });
                 builder
                    .UseAutofac()
                    .UseConsoleLifetime()
                    .UseSerilog();

                 await builder.Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }




        }
    }
}
