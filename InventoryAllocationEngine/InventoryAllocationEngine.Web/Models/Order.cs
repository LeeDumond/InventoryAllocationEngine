using System;
using System.Collections.Generic;

namespace InventoryAllocationEngine.Web.Models
{
   public class Order
   {
      public int Id { get; set; }
      public Guid CustomerId { get; set; }
      public DateTime DateReceived { get; set; }

      public virtual Customer Customer { get; set; }
      public virtual ICollection<OrderItem> OrderItems { get; set; } 
   }
}