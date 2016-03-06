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

         int quantityAllocated = 0;
         List<OrderItem> orderItemsSorted = product.OrderItems.ToList();

         if (allocationMethod == AllocationMethod.Simple)
         {

            orderItemsSorted = product.OrderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
            double percentage = (double)onHand / quantityOrdered;

            foreach (var orderItem in orderItemsSorted)
            {
               orderItem.QuantityAllocated = (int)(orderItem.QuantityOrdered * percentage);
            }

            quantityAllocated = product.OrderItems.Sum(oi => oi.QuantityAllocated);

         }

         if (allocationMethod == AllocationMethod.LargestOrderFirst)
         {
            orderItemsSorted = product.OrderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();

            int amountRemaining = onHand;
            int index = 0;

            while (amountRemaining > 0 && index+1 <= orderItemsSorted.Count)
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
         }

         int unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItemsSorted.First().QuantityAllocated = orderItemsSorted.First().QuantityAllocated + unallocated;
         }
      }
   }
}