using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ServiceCenterApp.Models;

public class ServiceCenterDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Work> Works { get; set; }
    public DbSet<WorkType> WorkTypes { get; set; }
    public DbSet<WorkDetail> WorkDetails { get; set; }
    public DbSet<UserWork> UserWorks { get; set; }
    public DbSet<Detail> Details { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockDetail> StockDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Role>()
            .Property(e => e.RoleName)
            .HasConversion(
                v => v.ToString(),
                v => (RoleName)Enum.Parse(typeof(RoleName), v))
            .HasMaxLength(15);

        modelBuilder
            .Entity<Status>()
            .Property(e => e.StatusName)
            .HasConversion(
                v => v.ToString(),
                v => (StatusName)Enum.Parse(typeof(StatusName), v))
            .HasMaxLength(15);
    }

    public ServiceCenterDbContext()
    {
        //comment after init db
        Database.Migrate();
        //ypa
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .SetBasePath(Directory.GetCurrentDirectory())
            .Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }
}