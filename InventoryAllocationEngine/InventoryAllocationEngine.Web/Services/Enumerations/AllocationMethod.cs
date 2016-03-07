using System.ComponentModel.DataAnnotations;

namespace InventoryAllocationEngine.Web.Services.Enumerations
{
   public enum AllocationMethod
   {
      //[Display(Name = "Unweighted")]
      //Unweighted,

      [Display(Name = "Favor Larger Orders")]
      LargestOrdersFirst,

      [Display(Name = "Favor Older Orders")]
      OldestOrdersFirst,

      [Display(Name = "Favor Larger Customers")]
      BestCustomersFirst
   }
}