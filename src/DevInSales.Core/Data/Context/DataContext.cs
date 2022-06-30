using System.Reflection;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Data.Context
{
    public class DataContext : IdentityDbContext<User>
    {
        public 
            DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
        
  
