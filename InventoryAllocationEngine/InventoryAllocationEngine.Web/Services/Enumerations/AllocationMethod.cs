using System.ComponentModel.DataAnnotations;

namespace InventoryAllocationEngine.Web.Services.Enumerations
{
   public enum AllocationMethod
   {
      [Display(Name = "Simple")]
      Simple,

      [Display(Name = "Largest Orders First")]
      LargestOrdersFirst,

      [Display(Name = "Oldest Orders First")]
      OldestOrdersFirst,

      [Display(Name = "Best Customers First")]
      BestCustomersFirst
   }
}