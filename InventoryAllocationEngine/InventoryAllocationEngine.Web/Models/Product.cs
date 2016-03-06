using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InventoryAllocationEngine.Web.Models
{
   [DebuggerDisplay("{Description}")]
   public class Product
   {
      public Guid Id { get; set; }
      public string Description { get; set; }
      public int QuantityAvailable { get; set; } 

      public virtual ICollection<OrderItem> OrderItems { get; set; } 
   }
}