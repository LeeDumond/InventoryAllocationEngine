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

      public void Allocate(Guid id, AllocationMethod allocationMethod, double weighting)
      {
         var product = dbContext.Products.Find(id);

         var onHand = product.QuantityAvailable;
         var quantityOrdered = product.OrderItems.Sum(oi => oi.QuantityOrdered);

         if (onHand > quantityOrdered)
         {
            return;
         }

         Allocate(product, allocationMethod);
      }

      private void Allocate(Product product, AllocationMethod allocationMethod)
      {
         List<OrderItem> orderItems;

         switch (allocationMethod)
         {
            case AllocationMethod.Simple:
               AllocateSimple(product);
               return;

            case AllocationMethod.LargestOrdersFirst:
               orderItems = product.OrderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
               break;

            case AllocationMethod.OldestOrdersFirst:
               orderItems = product.OrderItems.OrderBy(oi => oi.Order.DateReceived).ToList();
               break;

            case AllocationMethod.BestCustomersFirst:
               orderItems = product.OrderItems.OrderByDescending(oi => oi.Order.Customer.AverageAnnualVolume).ToList();
               break;

            default:
               AllocateSimple(product);
               return;
         }

         int quantityAllocated = 0;
         int onHand = product.QuantityAvailable;

         int amountRemaining = onHand;
         int index = 0;

         while (amountRemaining > 0 && index + 1 <= orderItems.Count)
         {
            OrderItem currentOrderItem = orderItems[index];

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
            orderItems.First().QuantityAllocated = orderItems.First().QuantityAllocated + unallocated;
         }
      }

      private void AllocateSimple(Product product)
      {
         var orderItems = product.OrderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
         int onHand = product.QuantityAvailable;
         int quantityOrdered = product.OrderItems.Sum(oi => oi.QuantityOrdered);

         double percentage = (double) onHand / quantityOrdered;

         foreach (var orderItem in orderItems)
         {
            orderItem.QuantityAllocated = (int) (orderItem.QuantityOrdered * percentage);
         }

         int quantityAllocated = product.OrderItems.Sum(oi => oi.QuantityAllocated);

         int unallocated = onHand - quantityAllocated;

         if (unallocated > 0)
         {
            orderItems.First().QuantityAllocated = orderItems.First().QuantityAllocated + unallocated;
         }
      }
   }
}