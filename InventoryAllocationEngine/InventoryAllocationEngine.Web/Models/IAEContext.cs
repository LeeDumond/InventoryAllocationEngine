using System.Data.Entity;

namespace InventoryAllocationEngine.Web.Models
{
   public class IAEContext : DbContext
   {
      public DbSet<Order> Orders { get; set; }
   }
}