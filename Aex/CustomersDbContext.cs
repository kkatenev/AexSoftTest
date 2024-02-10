using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection.Emit;

namespace Aex
{
    internal class CustomersDbContext : DbContext
    {
        public CustomersDbContext() 
            : base("Data Source=server;Initial Catalog=database;User ID=username;Password=password;")
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Определение отношений между сущностями
            modelBuilder.Entity<Customer>()
                .HasRequired(c => c.Manager)
                .WithMany()
                .HasForeignKey(c => c.ManagerID);

            modelBuilder.Entity<Order>()
                .HasRequired(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerID);
        }
    }
}