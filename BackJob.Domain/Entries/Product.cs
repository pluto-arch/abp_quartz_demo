using Volo.Abp.Domain.Entities;

namespace BackJob.Domain.Entries;

public class Product:Entity<int>
{
    public string Name { get; set; }

    public string Address { get; set; }
}