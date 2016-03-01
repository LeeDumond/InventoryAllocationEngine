using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryAllocationEngine.Web.Models;

namespace InventoryAllocationEngine.Web.Controllers
{
    public class CustomersController : Controller
    {
      private readonly IAEContext dbContext = new IAEContext();

        public ActionResult Index()
        {
            return View(dbContext.Customers.OrderBy(c => c.Name));
        }
    }
}