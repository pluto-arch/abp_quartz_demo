using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BackJob.EntityFrameworkCore;

public class BackJobDbContextFactory : IDesignTimeDbContextFactory<BackJobDbContext>
{
    public BackJobDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BackJobDbContext>()
            .UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=BackJobDb;Trusted_Connection=True;TrustServerCertificate=True");
        return new BackJobDbContext(builder.Options);
    }

}
