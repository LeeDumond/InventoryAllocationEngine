using System;

namespace InventoryAllocationEngine.Web.Models
{
   public class Product
   {
      public Guid Id { get; set; }
      public string Name { get; set; }
      public int QuantityAvailable { get; set; } 
   }
}