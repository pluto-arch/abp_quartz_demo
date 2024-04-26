using BackJob.Domain.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace BackJob.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class BackJobDbContext: AbpDbContext<BackJobDbContext>
    {

        


        /// <inheritdoc />
        public BackJobDbContext(DbContextOptions<BackJobDbContext> options) : base(options)
        {
        }

        public string Id { get; set; }

        public virtual  DbSet<Product> Product { get; set; }


        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ReplaceService<IModelCacheKeyFactory,BackJobModelCacheFactory>();
            optionsBuilder.ReplaceService<IModelCustomizer,BackJobModelCustomer>();
        }


        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(b =>
            {
                b.HasKey(x=>x.Id);
                b.Property(x=>x.Id).ValueGeneratedOnAdd();
                b.Property(x=>x.Name).IsRequired().HasMaxLength(50);
                b.Property(x=>x.Address).IsRequired().HasMaxLength(100);
            });
        }
    }


    public class ValueContext
    {
        public static AsyncLocal<string> CurrentId = new AsyncLocal<string>();
    }

    public class BackJobModelCacheFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime)
        {
            if (context is BackJobDbContext db)
            {
                return (context.GetType(), db.Id);
            }

            return context.GetType();
        }
    }

    public class BackJobModelCustomer :IModelCustomizer
    {
        /// <inheritdoc />
        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            if (context is BackJobDbContext db)
            {
                modelBuilder.Entity<Product>().ToTable($"Product{ValueContext.CurrentId.Value}");
            }
        }
    }
}
