using System.ComponentModel.DataAnnotations;

namespace InventoryAllocationEngine.Web.Services.Enumerations
{
   public enum AllocationMethod
   {
      [Display(Name = "Unweighted")]
      Unweighted,

      [Display(Name = "Largest Orders First")]
      LargestOrdersFirst,

      [Display(Name = "Oldest Orders First")]
      OldestOrdersFirst,

      [Display(Name = "Best Customers First")]
      BestCustomersFirst
   }
}