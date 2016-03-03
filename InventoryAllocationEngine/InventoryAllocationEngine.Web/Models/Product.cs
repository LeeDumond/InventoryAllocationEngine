using System;
using System.Collections.Generic;

namespace InventoryAllocationEngine.Web.Models
{
   public class Product
   {
      public Guid Id { get; set; }
      public string Description { get; set; }
      public int QuantityAvailable { get; set; } 

      public virtual ICollection<OrderItem> OrderItems { get; set; } 
   }
}