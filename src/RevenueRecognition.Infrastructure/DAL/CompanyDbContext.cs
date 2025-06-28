using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Models.Auth;
using RevenueRecognition.Models.Client;
using RevenueRecognition.Models.Contract;
using RevenueRecognition.Models.Discount;
using RevenueRecognition.Models.Software;
using RevenueRecognition.Models.Subscription;


namespace RevenueRecognition.Infrastructure.DAL;

public class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<Client> Clients { get; set; }
    public DbSet<CompanyClient> CompanyClients { get; set; }
    public DbSet<IndividualClient> IndividualClients { get; set; }

    public DbSet<Discount> Discounts { get; set; }

    public DbSet<ContractStatus> ContractStatuses { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<ContractPayment> ContractPayments { get; set; }
    public DbSet<UpdateType> UpdateTypes { get; set; }
    public DbSet<ContractUpdateType> ContractUpdateTypes { get; set; }
    public DbSet<DiscountContract> DiscountContracts { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<SoftwareVersion> SoftwareVersions { get; set; }

    public DbSet<SubscriptionStatus> SubscriptionStatuses { get; set; }
    public DbSet<RenewalPeriod> RenewalPeriods { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<DiscountSubscription> DiscountSubscriptions { get; set; }
    public DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Login = "admin", Password = "admin123", RoleId = 1 },
            new User { Id = 2, Login = "user", Password = "user123", RoleId = 2 }
        );

        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                Id = 1, Address = "123 Main St", Email = "company@example.com", IsLoyal = true,
                PhoneNumber = "123456789"
            },
            new Client
            {
                Id = 2, Address = "456 Elm St", Email = "individual@example.com", IsLoyal = false,
                PhoneNumber = "987654321"
            }
        );

        modelBuilder.Entity<CompanyClient>().HasData(
            new CompanyClient { Id = 1, ClientId = 1, KRS = "1234567890", Name = "Example Corp" }
        );

        modelBuilder.Entity<IndividualClient>().HasData(
            new IndividualClient
                { Id = 1, ClientId = 2, FirstName = "John", LastName = "Doe", PESEL = "12345678901", IsDeleted = false }
        );
        
        modelBuilder.Entity<ContractStatus>().HasData(
            new ContractStatus
                { Id = 1, Name = "Paid"},
            new ContractStatus
                { Id = 2, Name = "Active"},
            new ContractStatus
                { Id = 3, Name = "Cancelled"}
        );
        
        modelBuilder.Entity<SubscriptionStatus>().HasData(
            new SubscriptionStatus
                { Id = 1, Name = "Active"},
            new SubscriptionStatus
                { Id = 2, Name = "Suspended"}
        );
        
        

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Productivity" },
            new Category { Id = 2, Name = "Entertainment" }
        );

        modelBuilder.Entity<SoftwareVersion>().HasData(
            new SoftwareVersion
            {
                Id = 1, SoftwareId = 1, Description = "v1.0 release", ReleaseDate = new DateTime(2023, 1, 1),
                VersionNumber = "1.0"
            },
            new SoftwareVersion
            {
                Id = 2, SoftwareId = 2, Description = "v1.0 release", ReleaseDate = new DateTime(2023, 2, 1),
                VersionNumber = "1.0"
            }
        );

        modelBuilder.Entity<Software>().HasData(
            new Software
            {
                Id = 1, Name = "SuperProductivity", Description = "Task manager", CategoryId = 1, Cost = 99.99m,
                CurrentVersionId = 1
            },
            new Software
            {
                Id = 2, Name = "FunGame", Description = "Casual game", CategoryId = 2, Cost = 49.99m,
                CurrentVersionId = 2
            }
        );

        modelBuilder.Entity<UpdateType>().HasData(
            new UpdateType { Id = 1, Name = "Bug Fix" },
            new UpdateType { Id = 2, Name = "Feature" }
        );

        modelBuilder.Entity<Discount>().HasData(
            new Discount
            {
                Id = 1, Name = "Loyal Customer Discount", StartDate = null,
                EndDate = null, Value = 5m
            },
            new Discount
            {
                Id = 2, Name = "New Year Sale", StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 1, 31), Value = 10m
            },
            new Discount
            {
                Id = 3, Name = "Black Friday", StartDate = new DateTime(2024, 11, 25),
                EndDate = new DateTime(2024, 11, 30), Value = 20m
            }
        );

        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1,
                ClientId = 1,
                Price = 1000m,
                Paid = 500m,
                StartDate = new DateTime(2025, 6, 10),
                EndDate = new DateTime(2025, 7, 5),
                SoftwareVersionId = 1,
                YearsOfSupport = 1,
                ContractStatusId = 2
            }
        );
        
        modelBuilder.Entity<ContractPayment>().HasData(
            new ContractPayment
            {
                Id = 1,
                ContractId = 1,
                Amount = 300m,
                PaidAt = new DateTime(2025, 6, 15)
            },
            new ContractPayment
            {
                Id = 2,
                ContractId = 1,
                Amount = 200m,
                PaidAt = new DateTime(2025, 6, 20)
            }
        );


        modelBuilder.Entity<ContractUpdateType>().HasData(
            new ContractUpdateType { ContractId = 1, UpdateTypeId = 1 },
            new ContractUpdateType { ContractId = 1, UpdateTypeId = 2 }
        );

        modelBuilder.Entity<DiscountContract>().HasData(
            new DiscountContract { DiscountId = 1, ContractId = 1 }
        );

        modelBuilder.Entity<RenewalPeriod>().HasData(
            new RenewalPeriod { Id = 1, Name = "Monthly" },
            new RenewalPeriod { Id = 2, Name = "Yearly" }
        );

        modelBuilder.Entity<Subscription>().HasData(
            new Subscription
            {
                Id = 1,
                ClientId = 2,
                SoftwareId = 1,
                RenewalPeriodId = 1,
                Price = 9.99m,
                RegisterDate = new DateTime(2025, 4, 1),
                SubscriptionStatusId = 1
            }
        );
        
        modelBuilder.Entity<SubscriptionPayment>().HasData(
            new SubscriptionPayment
            {
                Id = 1,
                SubscriptionId = 1,
                Amount = 9.99m,
                PaidAt = new DateTime(2025, 4, 1)
            },
            new SubscriptionPayment
            {
                Id = 2,
                SubscriptionId = 1,
                Amount = 9.99m,
                PaidAt = new DateTime(2025, 5, 1)
            },
            new SubscriptionPayment
            {
                Id = 3,
                SubscriptionId = 1,
                Amount = 9.99m,
                PaidAt = new DateTime(2025, 6, 1)
            }
        );


        modelBuilder.Entity<DiscountSubscription>().HasData(
            new DiscountSubscription { DiscountId = 2, SubscriptionId = 1 }
        );
    }
}