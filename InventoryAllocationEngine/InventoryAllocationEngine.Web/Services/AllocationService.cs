using System;
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

      public void Allocate(Guid id, AllocationMethod allocationMethod, double weighting)
      {
         var product = dbContext.Products.Find(id);

         var onHand = product.QuantityAvailable;
         var quantityOrdered = product.OrderItems.Sum(oi => oi.QuantityOrdered);

         if (onHand > quantityOrdered)
         {
            return;
         }

         switch (allocationMethod)
         {
            case AllocationMethod.Simple:
               AllocateSimple(product);
               break;

            case AllocationMethod.LargestOrdersFirst:
               AllocateLargestOrdersFirst(product);
               break;

            case AllocationMethod.OldestOrdersFirst:
               AllocateOldestOrdersFirst(product);
               break;

            case AllocationMethod.BestCustomersFirst:
               AllocateBestCustomersFirst(product);
               break;

            default:
               AllocateSimple(product);
               break;
         }
      }

      private void AllocateBestCustomersFirst(Product product)
      {
         var quantityAllocated = 0;
         var orderItemsSorted = product.OrderItems.OrderByDescending(oi => oi.Order.Customer.AverageAnnualVolume).ToList();
         var onHand = product.QuantityAvailable;

         var amountRemaining = onHand;
         var index = 0;

         while (amountRemaining > 0 && index + 1 <= orderItemsSorted.Count)
         {
            var currentOrderItem = orderItemsSorted[index];

            if (amountRemaining > currentOrderItem.QuantityOrdered)
            {
               currentOrderItem.QuantityAllocated = currentOrderItem.QuantityOrdered;
               quantityAllocated += currentOrderItem.QuantityAllocated;
            }
            else
            {
               currentOrderItem.QuantityAllocated = amountRemaining;
               quantityAllocated += amountRemaining;
            }

            amountRemaining -= currentOrderItem.QuantityAllocated;

            index++;
         }

         var unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItemsSorted.First().QuantityAllocated = orderItemsSorted.First().QuantityAllocated + unallocated;
         }
      }

      private void AllocateOldestOrdersFirst(Product product)
      {
         var quantityAllocated = 0;
         var orderItemsSorted = product.OrderItems.OrderBy(oi => oi.Order.DateReceived).ToList();
         var onHand = product.QuantityAvailable;

         var amountRemaining = onHand;
         var index = 0;

         while (amountRemaining > 0 && index + 1 <= orderItemsSorted.Count)
         {
            var currentOrderItem = orderItemsSorted[index];

            if (amountRemaining > currentOrderItem.QuantityOrdered)
            {
               currentOrderItem.QuantityAllocated = currentOrderItem.QuantityOrdered;
               quantityAllocated += currentOrderItem.QuantityAllocated;
            }
            else
            {
               currentOrderItem.QuantityAllocated = amountRemaining;
               quantityAllocated += amountRemaining;
            }

            amountRemaining -= currentOrderItem.QuantityAllocated;

            index++;
         }

         var unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItemsSorted.First().QuantityAllocated = orderItemsSorted.First().QuantityAllocated + unallocated;
         }
      }

      private void AllocateLargestOrdersFirst(Product product)
      {
         var quantityAllocated = 0;
         var orderItemsSorted = product.OrderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
         var onHand = product.QuantityAvailable;

         var amountRemaining = onHand;
         var index = 0;

         while (amountRemaining > 0 && index + 1 <= orderItemsSorted.Count)
         {
            var currentOrderItem = orderItemsSorted[index];

            if (amountRemaining > currentOrderItem.QuantityOrdered)
            {
               currentOrderItem.QuantityAllocated = currentOrderItem.QuantityOrdered;
               quantityAllocated += currentOrderItem.QuantityAllocated;
            }
            else
            {
               currentOrderItem.QuantityAllocated = amountRemaining;
               quantityAllocated += amountRemaining;
            }

            amountRemaining -= currentOrderItem.QuantityAllocated;

            index++;
         }

         var unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItemsSorted.First().QuantityAllocated = orderItemsSorted.First().QuantityAllocated + unallocated;
         }
      }

      private void AllocateSimple(Product product)
      {
         var orderItemsSorted = product.OrderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
         var onHand = product.QuantityAvailable;
         var quantityOrdered = product.OrderItems.Sum(oi => oi.QuantityOrdered);

         var percentage = (double) onHand / quantityOrdered;

         foreach (var orderItem in orderItemsSorted)
         {
            orderItem.QuantityAllocated = (int) (orderItem.QuantityOrdered * percentage);
         }

         var quantityAllocated = product.OrderItems.Sum(oi => oi.QuantityAllocated);

         var unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItemsSorted.First().QuantityAllocated = orderItemsSorted.First().QuantityAllocated + unallocated;
         }
      }
   }
}