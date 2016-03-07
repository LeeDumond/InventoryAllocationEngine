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

         if (allocationMethod == AllocationMethod.Unweighted)
         {
            AllocateSimple(product.OrderItems.ToList(), product.QuantityAvailable);
            return;
         }

         // split up the orders
         List<OrderItem> orderItems = product.OrderItems.OrderBy(p => p.Id).ToList();
         List<OrderItem> orderItems1 = dbContext.Products.AsNoTracking().Single(p => p.Id == id).OrderItems.OrderBy(p => p.Id).ToList();
         List<OrderItem> orderItems2 = dbContext.Products.AsNoTracking().Single(p => p.Id == id).OrderItems.OrderBy(p => p.Id).ToList();

         foreach (var orderItem1 in orderItems1)
         {
            orderItem1.QuantityOrdered = (int) (orderItem1.QuantityOrdered * (1 - weighting));
         }

         for (int i = 0; i < orderItems2.Count; i++)
         {
            orderItems2[i].QuantityOrdered = orderItems[i].QuantityOrdered - orderItems1[i].QuantityOrdered;
         }

         int onHandFor1 = (int) (product.QuantityAvailable * (1 - weighting));
         int onHandFor2 = product.QuantityAvailable - onHandFor1;

         // orderItems1 gets Simple allocation
         orderItems1 = AllocateSimple(orderItems1, onHandFor1);

         // orderItems2 gets Complex allocation
         orderItems2 = AllocateComplex(orderItems2, onHandFor2, allocationMethod);

         // combine allocated quantities
         for (int i = 0; i < orderItems.Count; i++)
         {
            orderItems[i].QuantityAllocatedWeighted = orderItems1[i].QuantityAllocatedWeighted + orderItems2[i].QuantityAllocatedWeighted;
         }

      }

      private List<OrderItem> AllocateComplex(List<OrderItem> orderItems, int quantityAvailable, AllocationMethod allocationMethod)
      {
         switch (allocationMethod)
         {            
            case AllocationMethod.LargestOrdersFirst:
               orderItems = orderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
               break;

            case AllocationMethod.OldestOrdersFirst:
               orderItems = orderItems.OrderBy(oi => oi.Order.DateReceived).ToList();
               break;

            case AllocationMethod.BestCustomersFirst:
               orderItems = orderItems.OrderByDescending(oi => oi.Order.Customer.AverageAnnualVolume).ToList();
               break;

            default:
               return null;
         }

         int quantityAllocated = 0;

         int amountRemaining = quantityAvailable;
         int index = 0;

         while (amountRemaining > 0 && index + 1 <= orderItems.Count)
         {
            OrderItem currentOrderItem = orderItems[index];

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

      private List<OrderItem> AllocateSimple(List<OrderItem> orderItems, int quantityAvailable)
      {
         orderItems = orderItems.OrderByDescending(oi => oi.QuantityOrdered).ToList();
         
         int quantityOrdered = orderItems.Sum(oi => oi.QuantityOrdered);

         if (quantityOrdered > 0 && quantityAvailable > 0)
         {
            double percentage = (double)quantityAvailable / quantityOrdered;

            foreach (var orderItem in orderItems)
            {
               orderItem.QuantityAllocatedWeighted = (int)(orderItem.QuantityOrdered * percentage);
            }

            int quantityAllocated = orderItems.Sum(oi => oi.QuantityAllocatedWeighted);

            int unallocated = quantityAvailable - quantityAllocated;

            if (unallocated > 0)
            {
               orderItems.First().QuantityAllocatedWeighted = orderItems.First().QuantityAllocatedWeighted + unallocated;
            }
         }

         return orderItems.OrderBy(o => o.Id).ToList();
      }
   }
}