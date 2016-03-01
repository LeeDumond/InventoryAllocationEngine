using System;
using System.Collections.Generic;

namespace InventoryAllocationEngine.Web.Models
{
   public class Order
   {
      public Guid Id { get; set; }

      public virtual ICollection<OrderItem> OrderItems { get; set; } 
   }
}