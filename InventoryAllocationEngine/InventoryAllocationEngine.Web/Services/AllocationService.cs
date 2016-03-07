using System;
using System.Collections.Generic;
using System.Linq;
using InventoryAllocationEngine.Web.Models;
using InventoryAllocationEngine.Web.Services.Enumerations;

namespace InventoryAllocationEngine.Web.Services
{
   public class AllocationService
   {
      private readonly IAEContext dbContext;

      public AllocationService(IAEContext dbContext)
      {
         this.dbContext = dbContext;
      }

      public void CalculateUnweightedAllocation(Guid id)
      {
         var product = dbContext.Products.Find(id);

         CalculateUnweightedAllocation(product.OrderItems.ToList(), product.QuantityAvailable);
      }

      public void Allocate(Guid id, AllocationMethod allocationMethod, double weighting)
      {
         var product = dbContext.Products.Find(id);

         CalculateUnweightedAllocation(product.OrderItems.ToList(), product.QuantityAvailable);

         var onHand = product.QuantityAvailable;
         var quantityOrdered = product.OrderItems.Sum(oi => oi.QuantityOrdered);

         if (onHand > quantityOrdered)
         {
            AllocateUnweighted(product.OrderItems.ToList(), onHand);
            return;
         }

         // do weighted allocation
         var orderItems = product.OrderItems.OrderBy(p => p.Id).ToList();
         var orderItemsUnweighted = dbContext.Products.AsNoTracking().Single(p => p.Id == id).OrderItems.OrderBy(p => p.Id).ToList();
         var orderItemsWeighted = dbContext.Products.AsNoTracking().Single(p => p.Id == id).OrderItems.OrderBy(p => p.Id).ToList();

         foreach (var orderItem in orderItemsUnweighted)
         {
            orderItem.QuantityOrdered = (int) (orderItem.QuantityOrdered * (1 - weighting));
         }

         for (var i = 0; i < orderItemsWeighted.Count; i++)
         {
            orderItemsWeighted[i].QuantityOrdered = orderItems[i].QuantityOrdered - orderItemsUnweighted[i].QuantityOrdered;
         }

         var onHandForUnweigthted = (int) (product.QuantityAvailable * (1 - weighting));
         var onHandForWeighted = product.QuantityAvailable - onHandForUnweigthted;

         orderItemsUnweighted = AllocateUnweighted(orderItemsUnweighted, onHandForUnweigthted);

         orderItemsWeighted = AllocateWeighted(orderItemsWeighted, onHandForWeighted, allocationMethod);

         for (var i = 0; i < orderItems.Count; i++)
         {
            orderItems[i].QuantityAllocatedWeighted = orderItemsUnweighted[i].QuantityAllocatedWeighted + orderItemsWeighted[i].QuantityAllocatedWeighted;
         }
      }

      private void CalculateUnweightedAllocation(List<OrderItem> orderItems, int quantityAvailable)
      {
         orderItems = orderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();

         var quantityOrdered = orderItems.Sum(oi => oi.QuantityOrdered);

         if (quantityOrdered > 0 && quantityAvailable > 0)
         {
            var percentage = (double) quantityAvailable / quantityOrdered;

            if (percentage >= 1)
            {
               percentage = 1;
            }

            foreach (var orderItem in orderItems)
            {
               orderItem.QuantityAllocatedUnweighted = (int) (orderItem.QuantityOrdered * percentage);
            }

            if (percentage < 1)
            {
               var quantityAllocated = orderItems.Sum(oi => oi.QuantityAllocatedUnweighted);

               var unallocated = quantityAvailable - quantityAllocated;

               if (unallocated > 0)
               {
                  orderItems.First().QuantityAllocatedUnweighted = orderItems.First().QuantityAllocatedUnweighted + unallocated;
               }
            }
         }
      }

      private List<OrderItem> AllocateUnweighted(List<OrderItem> orderItems, int quantityAvailable)
      {
         orderItems = orderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();

         var quantityOrdered = orderItems.Sum(oi => oi.QuantityOrdered);

         if (quantityOrdered > 0 && quantityAvailable > 0)
         {
            var percentage = (double) quantityAvailable / quantityOrdered;

            if (percentage >= 1)
            {
               percentage = 1;
            }

            foreach (var orderItem in orderItems)
            {
               orderItem.QuantityAllocatedWeighted = (int) (orderItem.QuantityOrdered * percentage);
            }

            if (percentage < 1)
            {
               var quantityAllocated = orderItems.Sum(oi => oi.QuantityAllocatedWeighted);

               var unallocated = quantityAvailable - quantityAllocated;

               if (unallocated > 0)
               {
                  orderItems.First().QuantityAllocatedWeighted = orderItems.First().QuantityAllocatedWeighted + unallocated;
               }
            }
         }

         return orderItems.OrderBy(o => o.Id).ToList();
      }

      private List<OrderItem> AllocateWeighted(List<OrderItem> orderItems, int quantityAvailable, AllocationMethod allocationMethod)
      {
         switch (allocationMethod)
         {
            case AllocationMethod.LargestOrdersFirst:
               orderItems = orderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
               break;

            case AllocationMethod.OldestOrdersFirst:
               orderItems = orderItems.OrderBy(oi => oi.Order.DateReceived).ToList();
               break;

            case AllocationMethod.HighestVolumeCustomersFirst:
               orderItems = orderItems.OrderByDescending(oi => oi.Order.Customer.AverageAnnualVolume).ToList();
               break;

            case AllocationMethod.QuickPayCustomersFirst:
               orderItems = orderItems.OrderBy(oi => oi.Order.Customer.AccountsPayableAge).ToList();
               break;

            case AllocationMethod.CreditWorthyCustomersFirst:
               foreach (var orderItem in orderItems.Where(orderItem => orderItem.Order.Customer.DUNSScore == null))
               {
                  orderItem.Order.Customer.DUNSScore = 500;
               }
               orderItems = orderItems.OrderByDescending(oi => oi.Order.Customer.DUNSScore).ToList();
               break;

            default:
               throw new NotImplementedException("'" + allocationMethod + "' is not implemented.");
         }

         var quantityAllocated = 0;

         var amountRemaining = quantityAvailable;
         var index = 0;

         while (amountRemaining > 0 && index + 1 <= orderItems.Count)
         {
            var currentOrderItem = orderItems[index];

            if (amountRemaining > currentOrderItem.QuantityOrdered)
            {
               currentOrderItem.QuantityAllocatedWeighted = currentOrderItem.QuantityOrdered;
               quantityAllocated += currentOrderItem.QuantityAllocatedWeighted;
            }
            else
            {
               currentOrderItem.QuantityAllocatedWeighted = amountRemaining;
               quantityAllocated += amountRemaining;
            }

            amountRemaining -= currentOrderItem.QuantityAllocatedWeighted;
            index++;
         }

         var unallocated = quantityAvailable - quantityAllocated;

         if (unallocated > 0)
         {
            orderItems.First().QuantityAllocatedWeighted = orderItems.First().QuantityAllocatedWeighted + unallocated;
         }

         return orderItems.OrderBy(o => o.Id).ToList();
      }
   }
}