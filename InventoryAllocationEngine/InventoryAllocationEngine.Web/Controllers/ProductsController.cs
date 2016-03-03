using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryAllocationEngine.Web.Models;

namespace InventoryAllocationEngine.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IAEContext dbContext = new IAEContext();

        public ActionResult Index()
        {
            return View(dbContext.Products.OrderBy(p => p.Description));
        }

       public ActionResult Details(Guid id)
       {
          return View(dbContext.Products.Find(id));
       }

      [HttpPost]
      public ActionResult Allocate(string allocationMethod)
      {
         return View("Index", dbContext.Products.OrderBy(p => p.Description));
      }
   }
}