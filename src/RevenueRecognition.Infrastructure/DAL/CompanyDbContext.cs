using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Models.Auth;
using RevenueRecognition.Models.Client;
using RevenueRecognition.Models.Contract;
using RevenueRecognition.Models.Discount;
using RevenueRecognition.Models.Software;
using RevenueRecognition.Models.Subscription;

// using SoftwareVersion = RevenueRecognition.Models.Entities.Software.SoftwareVersion;

namespace RevenueRecognition.Infrastructure.DAL;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<Client> Clients { get; set; }
    public DbSet<CompanyClient> CompanyClients { get; set; }
    public DbSet<IndividualClient> IndividualClients { get; set; }

    public DbSet<Discount> Discounts { get; set; }

    public DbSet<Contract> Contracts { get; set; }
    public DbSet<UpdateType> UpdateTypes { get; set; }
    public DbSet<ContractUpdateType> ContractUpdateTypes { get; set; }
    public DbSet<DiscountContract> DiscountContracts { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<SoftwareVersion> SoftwareVersions { get; set; }

    public DbSet<RenewalPeriod> RenewalPeriods { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<DiscountSubscription> DiscountSubscriptions { get; set; }

 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
    }
}
