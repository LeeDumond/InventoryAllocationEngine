﻿using System;
using System.Collections.Generic;

namespace InventoryAllocationEngine.Web.Models
{
   public class OrderItem
   {
      public Guid Id { get; set; }
      public int OrderId { get; set; }
      public Guid ProductId { get; set; }
      public int Quantity { get; set; }

      public virtual Order Order { get; set; }
      public virtual Product Product { get; set; } 
   }
}