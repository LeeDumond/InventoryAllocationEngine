using System;
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
      public decimal UnitPrice { get; set; }

      [NotMapped]
      public int QuantityAllocatedWeighted { get; set; }

      [NotMapped]
      public int QuantityAllocatedUnweighted { get; set; }

      [NotMapped]
      public decimal WeightedExtension
      {
         get { return UnitPrice * QuantityAllocatedWeighted; }
      }

      [NotMapped]
      public decimal UnweightedExtension
      {
         get { return UnitPrice * QuantityAllocatedUnweighted; }
      }

      public virtual Order Order { get; set; }
      public virtual Product Product { get; set; } 
   }
}