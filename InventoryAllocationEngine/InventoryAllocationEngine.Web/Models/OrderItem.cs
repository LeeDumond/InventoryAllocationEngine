﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace InventoryAllocationEngine.Web.Models
{
   public class OrderItem
   {
      [DebuggerDisplay("{Product.Description} - {Order.Customer.Name} - {QuantityOrdered} ordered")]
      public Guid Id { get; set; }
      public int OrderId { get; set; }
      public Guid ProductId { get; set; }
      public int QuantityOrdered { get; set; }

      [NotMapped]
      public int QuantityAllocated { get; set; }

      public virtual Order Order { get; set; }
      public virtual Product Product { get; set; } 
   }
}