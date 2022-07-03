using System.Reflection;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Data.Context
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
              new IdentityRole {
                Id = "B83F4D09-88FD-4F39-956D-8594EAD05B07",
                Name = "Admin",
                NormalizedName = "ADMIN"
              },
              new IdentityRole {
                Id = "B16E8CBA-A5E2-4997-9CB8-CA45CC641953",
                Name = "Manager",
                NormalizedName = "MANAGER"
              },
              new IdentityRole {
                Id = "32AB2538-8F9D-4509-A8E6-43BF428A3C71",
                Name = "User",
                NormalizedName = "USER"
              }
            });

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "933224A3-0F24-478B-BB6E-E337C0E5BC66",
                    UserName = "admin@devin.com",
                    NormalizedUserName = "ADMIN@DEVIN.COM",
                    Email = "admin@devin.com",
                    PasswordHash = hasher.HashPassword(null, "Temp@2022"),
                    Nome = "admin"
                },
                new User
                {
                    Id = "D998A0EE-C00B-4CFF-B35E-3DD2DA3CE74B",
                    UserName = "manager@devin.com",
                    NormalizedUserName = "MANAGER@DEVIN.COM",
                    Email = "manager@devin.com",
                    PasswordHash = hasher.HashPassword(null, "Temp@2022"),
                    Nome = "manager"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "B83F4D09-88FD-4F39-956D-8594EAD05B07",
                    UserId = "933224A3-0F24-478B-BB6E-E337C0E5BC66"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "B16E8CBA-A5E2-4997-9CB8-CA45CC641953",
                    UserId = "D998A0EE-C00B-4CFF-B35E-3DD2DA3CE74B"
                }
            );
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses{ get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }

    }
}
        
  
