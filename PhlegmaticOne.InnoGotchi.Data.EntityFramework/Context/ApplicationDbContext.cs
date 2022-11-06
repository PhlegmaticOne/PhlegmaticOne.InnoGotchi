using Microsoft.EntityFrameworkCore;

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder
    //        .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=inno-gotchies-db;Integrated Security=True;");
    //    base.OnConfiguring(optionsBuilder);
    //}
}