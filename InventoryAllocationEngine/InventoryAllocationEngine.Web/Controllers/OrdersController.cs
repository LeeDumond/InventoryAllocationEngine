using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryAllocationEngine.Web.Models;

namespace InventoryAllocationEngine.Web.Controllers
{
   public class OrdersController : Controller
   {
      private readonly IAEContext dbContext = new IAEContext();

      public ActionResult Index()
      {
         return View(dbContext.Orders.OrderBy(o => o.DateReceived));
      }

      
   }
}