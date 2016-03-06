using System.ComponentModel.DataAnnotations;

namespace InventoryAllocationEngine.Web.Services
{
   public enum AllocationMethod
   {
      [Display(Name = "Simple")]
      Simple,

      [Display(Name = "Largest Orders First")]
      LargestOrdersFirst,

      [Display(Name = "Oldest Orders First")]
      OldestOrdersFirst
   }
}