using Microsoft.EntityFrameworkCore;
using System.Data;
using VDCD.Common.Model;

namespace VDCD.DL.ConnectDB
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }

        public IDbConnection CreateConnection()
        {
            return Database.GetDbConnection();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table mappings
            modelBuilder.Entity<Product>().ToTable("product");
            modelBuilder.Entity<Unit>().ToTable("unit");
            modelBuilder.Entity<Category>().ToTable("category");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<OrderDetail>().ToTable("order_detail");
            modelBuilder.Entity<Customer>().ToTable("customer");
            modelBuilder.Entity<User>().ToTable("users");

            // Convert all column names to lowercase to match PostgreSQL unquoted names
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }
            }

            // Manual calculations are used instead of database computed columns
            // FinalAmount and SubTotal are calculated in BLOrder.cs
        }
    }
}