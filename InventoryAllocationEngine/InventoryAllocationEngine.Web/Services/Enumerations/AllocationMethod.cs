using System.ComponentModel.DataAnnotations;

namespace InventoryAllocationEngine.Web.Services.Enumerations
{
   public enum AllocationMethod
   {
      [Display(Name = "Favor Most Profitable Orders")] MostProfitableOrdersFirst,

      [Display(Name = "Favor Larger Orders")] LargestOrdersFirst,

      [Display(Name = "Favor Earlier Orders")] OldestOrdersFirst,

      [Display(Name = "Favor High-Volume Customers")] HighestVolumeCustomersFirst,

      [Display(Name = "Favor Quick-Paying Customers")] QuickPayCustomersFirst,

      [Display(Name = "Favor Credit-Worthy Customers")] CreditWorthyCustomersFirst
   }
}