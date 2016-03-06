using System.ComponentModel.DataAnnotations;

namespace InventoryAllocationEngine.Web.Services
{
   public enum AllocationMethod
   {
      [Display(Name = "Simple")]
      Simple,

      [Display(Name = "Largest Order First")]
      LargestOrderFirst
   }
}