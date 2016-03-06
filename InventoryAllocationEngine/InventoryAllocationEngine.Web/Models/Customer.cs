using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InventoryAllocationEngine.Web.Models
{
   [DebuggerDisplay("{Name}")]
   public class Customer
   {
      public Guid Id { get; set; }

      public string Name { get; set; }

      public virtual ICollection<Order> Orders { get; set; } 
   }
}