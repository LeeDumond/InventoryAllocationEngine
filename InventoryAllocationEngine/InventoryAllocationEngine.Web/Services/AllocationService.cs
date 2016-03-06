using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using InventoryAllocationEngine.Web.Models;

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
         Product product = dbContext.Products.Find(id);

         int onHand = product.QuantityAvailable;
         int quantityOrdered = product.OrderItems.Sum(oi => oi.QuantityOrdered);

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

            default:
               AllocateSimple(product);
               break;
         }
      }

      private void AllocateOldestOrdersFirst(Product product)
      {
         int quantityAllocated = 0;
         var orderItemsSorted = product.OrderItems.OrderBy(oi => oi.Order.DateReceived).ToList();
         int onHand = product.QuantityAvailable;

         int amountRemaining = onHand;
         int index = 0;

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

         int unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItemsSorted.First().QuantityAllocated = orderItemsSorted.First().QuantityAllocated + unallocated;
         }
      }

      private void AllocateLargestOrdersFirst(Product product)
      {
         int quantityAllocated = 0;
         var orderItemsSorted = product.OrderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
         int onHand = product.QuantityAvailable;

         int amountRemaining = onHand;
         int index = 0;

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

         int unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItemsSorted.First().QuantityAllocated = orderItemsSorted.First().QuantityAllocated + unallocated;
         }
      }

      private void AllocateSimple(Product product)
      {
         var orderItemsSorted = product.OrderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
         int onHand = product.QuantityAvailable;
         int quantityOrdered = product.OrderItems.Sum(oi => oi.QuantityOrdered);

         double percentage = (double) onHand / quantityOrdered;

         foreach (var orderItem in orderItemsSorted)
         {
            orderItem.QuantityAllocated = (int) (orderItem.QuantityOrdered * percentage);
         }

         var quantityAllocated = product.OrderItems.Sum(oi => oi.QuantityAllocated);

         int unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItemsSorted.First().QuantityAllocated = orderItemsSorted.First().QuantityAllocated + unallocated;
         }
      }
   }
}