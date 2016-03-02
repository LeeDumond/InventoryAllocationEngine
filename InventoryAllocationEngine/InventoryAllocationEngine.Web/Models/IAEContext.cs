using System.Data.Entity;

namespace InventoryAllocationEngine.Web.Models
{
   public class IAEContext : DbContext
   {
      public DbSet<Order> Orders { get; set; }
      public DbSet<Customer> Customers { get; set; } 
      public DbSet<Product> Products { get; set; } 
   }
}