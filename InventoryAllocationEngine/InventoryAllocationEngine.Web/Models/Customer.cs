using System;
using System.Collections.Generic;

namespace InventoryAllocationEngine.Web.Models
{
   public class Customer
   {
      public Guid Id { get; set; }
      public string Name { get; set; }

      public virtual ICollection<Order> Orders { get; set; } 
   }
}